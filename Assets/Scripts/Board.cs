using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

public class Board : NetBehaviour {
  private class BoardState {
    private List<Square> _boardSquares;

    public BoardState(Square[] squares, Symbol symbolFill) {
      _boardSquares = new List<Square>(squares);

      for(int i = 0; i < _boardSquares.Count; i++) {
        _boardSquares[i].currentSymbol = symbolFill;
      }
    }

    public int IndexOf(Square square) => _boardSquares.IndexOf(square);
    public void fillBoard(Symbol fill) => _boardSquares.ForEach(square => square.currentSymbol = fill);

    public Symbol this[int i] {
      get => _boardSquares[i].currentSymbol;
      set {
        _boardSquares[i].currentSymbol = value;
      }

    }
  }

  [Header("Game Logic")]
  public Symbol startingPlayer = Symbol.None;

  // Custom handling when syncing
  private BoardState _boardState;
  private Dictionary<ulong, PlayerState> _players = new Dictionary<ulong, PlayerState>(2);

  // Netowrk Variables
  [SerializeField]
  private NetworkVariable<GameState> _gameState = new NetworkVariable<GameState>(new NetworkVariableSettings {
    WritePermission = NetworkVariablePermission.ServerOnly,
    ReadPermission = NetworkVariablePermission.Everyone
  });

  [SerializeField]
  private NetworkVariable<Symbol> _winner = new NetworkVariable<Symbol>(new NetworkVariableSettings {
    WritePermission = NetworkVariablePermission.ServerOnly,
    ReadPermission = NetworkVariablePermission.Everyone
  });

  [SerializeField]
  private NetworkVariable<Symbol> _currentPlayer = new NetworkVariable<Symbol>(new NetworkVariableSettings {
    WritePermission = NetworkVariablePermission.ServerOnly,
    ReadPermission = NetworkVariablePermission.Everyone
  });

  private void Start() {
    _boardState = new BoardState(GetComponentsInChildren<Square>(), Symbol.None);

    NetworkManager.Singleton.OnServerStarted += initializeServer;
    NetworkManager.Singleton.OnClientConnectedCallback += givePlayerToClient;
    NetworkManager.Singleton.OnClientDisconnectCallback += removePlayer;
  }

  public void setInitialGameState() {
    _boardState.fillBoard(Symbol.None);
    _players.Clear();
    if(IsServer) {
      _gameState.Value = GameState.NotStarted;
      _winner.Value = Symbol.None;
      _currentPlayer.Value = startingPlayer;
    }
  }

  private void initializeServer() {
    setInitialGameState();
    if(IsHost) {
      givePlayerToClient(OwnerClientId);
    }
  }

  private void givePlayerToClient(ulong playerID) {
    if(IsThinClient) {
      setInitialGameState();
    }

    const int maxPlayers = 2;
    if(!IsServer) {
      return;
    }

    Debug.Log("New client connection");
    if(_players.Count < maxPlayers) {
      var player = NetworkManagerController.playerFromID(playerID);
      player.playerSymbol = _players.Count == 0? startingPlayer : startingPlayer.other();

      _players.Add(playerID, player);

      _gameState.Value = _players.Count == maxPlayers? GameState.InProgress : GameState.NotStarted;
    } else {
      Debug.Log("More than 2 Clients, client will not receive Symbol");
    }
  }

  private void removePlayer(ulong playerID) {
    if(_players.Remove(playerID)) {
      _gameState.Value = GameState.Ended;
      NetworkManager.Singleton.StopServer();
    }
  }

  public void clickSquare(Square square) {
    if(!IsClient) {
      return;
    }

    requestClickSquare_ServerRpc(_boardState.IndexOf(square));
  }

  private void playOnSquare(Symbol player, int square) {
    if(_gameState.Value == GameState.InProgress && _currentPlayer.Value == player && _boardState[square] == Symbol.None) {
      // Update board and game Serverside
      _boardState[square] = player;
      _currentPlayer.Value = player.other();

      updateClientBoard_ClientRpc(square, player);

      // winner = checkWinner
      // if winner || no moves
      // endgame
    }

    Debug.Log($"Move from {player} on square {square} is illegal");
  }

  /*********** RPCs ***********/
  [ServerRpc(RequireOwnership = false)]
  private void requestClickSquare_ServerRpc(int squareIndex, ServerRpcParams rpcParams = default) {
    if(!IsServer) {
      return;
    }

    if(_players.TryGetValue(rpcParams.Receive.SenderClientId, out var player)) {
      playOnSquare(player.playerSymbol, squareIndex);
    }
  }

  [ClientRpc]
  private void updateClientBoard_ClientRpc(int square, Symbol symbol) {
    _boardState[square] = symbol;
  }
}
