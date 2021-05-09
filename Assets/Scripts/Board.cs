using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

public class Board : NetBehaviour {
  public class BoardState {
    private List<Square> _boardSquares;

    public BoardState(Square[] squares, Symbol symbolFill) {
      _boardSquares = new List<Square>(squares);

      for(int i = 0; i < _boardSquares.Count; i++) {
        _boardSquares[i].currentSymbol = symbolFill;
      }
    }
    public void fillBoard(Symbol fill) => _boardSquares.ForEach(square => square.currentSymbol = fill);

    public Symbol this[int i] {
      get => _boardSquares[i].currentSymbol;
      set {
        _boardSquares[i].currentSymbol = value;
      }
    }

    public int Count {
      get => _boardSquares.Count;
    }

    public bool Contains(Symbol filter) {
      foreach(var square in _boardSquares) {
        if(square.currentSymbol == filter) {
          return true;
        }
      }

      return false;
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

  // Delegates
  public delegate void GameStartedDelegate();
  public delegate void GameEndedDelegate(Symbol winner);
  public delegate void GameInterruptedDelegate();

  public static GameStartedDelegate OnGameStarted;
  public static GameEndedDelegate OnGameEnded;
  public static GameInterruptedDelegate OnGameInterrupted;

  private void Start() {
    var squares = GetComponentsInChildren<Square>();
    _boardState = new BoardState(squares, Symbol.None);

    // Create bound functions for each click event and its respective index
    for(int i = 0; i < squares.Length; i++) {
      var closure = i;
      squares[i].OnClick += () => {
        if(!IsClient) {
          return;
        }
        requestClickSquare_ServerRpc(closure);
      };
    }

    NetworkManager.Singleton.OnServerStarted += initializeServer;
    NetworkManager.Singleton.OnClientConnectedCallback += givePlayerToClient;
    NetworkManager.Singleton.OnClientDisconnectCallback += removePlayer;

    WaitMenu.OnCancel += () => {
      if(IsServer) {
        gameInterrupted_ClientRpc();
      }
    };
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

    if(_players.Count < maxPlayers) {
      var player = NetworkManagerController.playerFromID(playerID);
      player.playerSymbol = _players.Count == 0? startingPlayer : startingPlayer.other();

      _players.Add(playerID, player);

      _gameState.Value = _players.Count == maxPlayers? GameState.InProgress : GameState.NotStarted;
    } else {
      Debug.Log("More than 2 Clients, new client will not receive Symbol");
    }
  }

  private void removePlayer(ulong playerID) {
    if(_players.Remove(playerID)) {
      StartCoroutine(endGame());
    }
  }

  private void playOnSquare(Symbol player, int square) {
    if(_gameState.Value == GameState.InProgress && _currentPlayer.Value == player && _boardState[square] == Symbol.None) {
      // Update board and game Serverside
      _boardState[square] = player;
      _currentPlayer.Value = player.other();

      updateClientBoard_ClientRpc(square, player);

      _winner.Value = checkWinner(_boardState);
      if(_winner.Value != Symbol.None || !_boardState.Contains(Symbol.None)) {
        StartCoroutine(endGame());
      }
    }
  }

  private IEnumerator endGame() {
    if(!IsServer) {
      yield break;
    }

    Debug.Log("Endgame");
    _gameState.Value = GameState.Ended;

    yield return new WaitForSeconds(2);

    if(_winner.Value != Symbol.None) {
      // Win
      gameEnded_ClientRpc(_winner.Value);

    } else if(!_boardState.Contains(Symbol.None)) {
      // Draw
      gameEnded_ClientRpc(Symbol.None);

    } else {
      // Client disconnect
      gameInterrupted_ClientRpc();
    }
  }

  public static Symbol checkWinner(BoardState board) {
    int boardSize = (int) Mathf.Sqrt(board.Count);

    List<int> countRows = new List<int>(boardSize);
    List<int> countColumns = new List<int>(boardSize);

    for(int i = 0; i < boardSize; i++) {
      countRows.Add(0);
      countColumns.Add(0);
    }

    int mainDiagonal = 0;
    int otherDiagonal = 0;

    // Add up score of Rows, Columns and Diagonals
    for(int i = 0; i < board.Count; i++) {
      int row = i / boardSize;
      int column = i % boardSize;

      // Rows
      countRows[row] += (int) board[i];
      if(Mathf.Abs(countRows[row]) == boardSize) {
        return (Symbol)(countRows[row] / boardSize);
      }

      // Columns
      countColumns[column] += (int) board[i];
      if(Mathf.Abs(countColumns[column]) == boardSize) {
        return (Symbol)(countColumns[column] / boardSize);
      }

      // Main Diagonal
      if(row == column) {
        mainDiagonal += (int) board[i];
        if(Mathf.Abs(mainDiagonal) == boardSize) {
          return (Symbol) (mainDiagonal / boardSize);
        }
      }

      // Other Diagonal
      if(row + column == boardSize - 1) {
        otherDiagonal += (int) board[i];
        if(Mathf.Abs(otherDiagonal) == boardSize) {
          return (Symbol) (otherDiagonal / boardSize);
        }
      }
    }

    return Symbol.None;
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

  [ClientRpc]
  private void gameStarted_ClientRpc() => OnGameStarted?.Invoke();

  [ClientRpc]
  private void gameEnded_ClientRpc(Symbol winner) => OnGameEnded?.Invoke(winner);

  [ClientRpc]
  private void gameInterrupted_ClientRpc() => OnGameInterrupted?.Invoke();
}
