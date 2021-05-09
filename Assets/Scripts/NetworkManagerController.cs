using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Transports;
using MLAPI.Transports.UNET;
using MLAPI.Transports.PhotonRealtime;

public enum NetworkTransportTypes {
  Direct,
  Relayed
}

public class NetworkManagerController : MonoBehaviour {
  private NetworkManager _netManager = null;
  private NetworkTransport _ipTransport = null;
  private NetworkTransport _relayedTransport = null;

  private NetworkTransport _currentTransport {
    get {
      return _netManager?.NetworkConfig.NetworkTransport;
    }
    set {
      if(_netManager != null) {
        _netManager.NetworkConfig.NetworkTransport = value;
      }
    }
  }

  private void Awake() {
    _netManager = GetComponent<NetworkManager>();
    _ipTransport = GetComponent<UNetTransport>();
    _relayedTransport = GetComponent<PhotonRealtimeTransport>();
  }

  public static PlayerState playerFromID(ulong userID) {
    NetworkManager.Singleton.ConnectedClients.TryGetValue(userID, out var client);
    return client?.PlayerObject.GetComponent<PlayerState>();
  }
}