﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Transports;
using MLAPI.Transports.UNET;
using MLAPI.Transports.PhotonRealtime;

public enum NetworkTransportTypes {
  Direct,
  Relayed,
  None
}

public class NetworkManagerController : MonoBehaviour {
  // Events
  public delegate void OnConnectedDelegate(bool isHost);
  public static OnConnectedDelegate OnConnected;

  private const ushort _port = 53658;

  private NetworkManager _netManager = null;
  private UNetTransport _ipTransport = null;
  private PhotonRealtimeTransport _relayedTransport = null;

  private NetworkTransport _transport {
    get {
      return _netManager?.NetworkConfig.NetworkTransport;
    }
    set {
      if(_netManager != null) {
        _netManager.NetworkConfig.NetworkTransport = value;
      }
    }
  }

  private NetworkTransportTypes _transportType {
    get {
      var curr = _transport;
      if(curr == null) {
        return NetworkTransportTypes.None;
      }

      return curr == _ipTransport? NetworkTransportTypes.Direct : NetworkTransportTypes.Relayed;
    }
    set {
      switch(value) {
        case NetworkTransportTypes.Direct:
          _transport = _ipTransport;
          break;

        case NetworkTransportTypes.Relayed:
          _transport = _relayedTransport;
          break;

        case NetworkTransportTypes.None:
          _transport = null;
          break;
      }
    }
  }

  private void Start() {
    _netManager = GetComponent<NetworkManager>();
    _ipTransport = GetComponent<UNetTransport>();
    _relayedTransport = GetComponent<PhotonRealtimeTransport>();

    ConnectionMenu.OnStartGame += startGameConnection;
    WaitMenu.OnCancel += disconnect;
    Board.OnGameEnded += (Symbol winner) => disconnect();
    Board.OnGameInterrupted += disconnect;
  }

  private void startGameConnection(bool isHost, NetworkTransportTypes transportType, string address) {
    _transportType = transportType;
    if(_transport is UNetTransport unet) {
      unet.ConnectAddress = address;
      unet.ConnectPort = _port;
      unet.ServerListenPort = _port;

      if(isHost) {
        unet.ConnectAddress = "127.0.0.1";
      }

    } else if(_transport is PhotonRealtimeTransport photon) {
      photon.RoomName = address;
    }

    if(isHost) {
      _netManager.StartHost();
    } else {
      _netManager.StartClient();
    }

    OnConnected?.Invoke(isHost);
  }

  private void disconnect() {
    if(_netManager.IsHost) {
      _netManager.StopHost();
    } else if(_netManager.IsServer) {
      _netManager.StopServer();
    } else if(_netManager.IsClient) {
      _netManager.StopClient();
    }
  }

  public static PlayerState playerFromID(ulong userID) {
    NetworkManager.Singleton.ConnectedClients.TryGetValue(userID, out var client);
    return client?.PlayerObject.GetComponent<PlayerState>();
  }
}