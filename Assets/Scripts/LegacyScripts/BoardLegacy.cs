using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLegacy : MonoBehaviour {

  // Game Logic
  public Symbol startingPlayer = Symbol.None;

  private GameState _gameState;
  private Symbol _winner;
  private Symbol _currentPlayer;
  private List<Symbol> _boardState;
  private List<Square> _boardSquares;

  // AI Stuff
  public float AIWait = 0;
  public List<Symbol> AIPlayers = new List<Symbol>();
  public List<AIType> AIBehaviors = new List<AIType>();

  public GameObject canvas;
  public GameObject endMenu;
  private EndMenu _endMenu;

  void Awake() {
    _boardSquares = new List<Square>(GetComponentsInChildren<Square>());

    _endMenu = endMenu.GetComponent<EndMenu>();
  }

  void Start() {
    initializeGame();
  }

  private void initializeGame() {
    // Initialize Squares and Game Objects
    _boardState = new List<Symbol>();

    foreach (var square in _boardSquares) {
      _boardState.Add(Symbol.None);
      square.currentSymbol = Symbol.None;
    }

    _gameState = GameState.NotStarted;
    _currentPlayer = startingPlayer;
    _winner = Symbol.None;
  }

  public void startGame() {
    _gameState = GameState.InProgress;

    // Do first AI move, if necessary
    StartCoroutine(doAIMove());
  }

  public void setPlayerOneAI(AIType difficulty) {
    setPlayerAI(_currentPlayer, difficulty);
  }

  public void setPlayerTwoAI(AIType difficulty) {
    setPlayerAI(_currentPlayer.other(), difficulty);
  }

  private void setPlayerAI(Symbol player, AIType difficulty) {
    int index;
    if(difficulty == AIType.Human) { // Remove AI player, if existing
      index = AIPlayers.IndexOf(player);
      if(index != -1) {
        AIPlayers.RemoveAt(index);
        AIBehaviors.RemoveAt(index);
      }
    } else {
      index = AIPlayers.IndexOf(player);
      if(index != -1) {
        AIBehaviors[index] = difficulty;
      } else {
        AIPlayers.Add(player);
        AIBehaviors.Add(difficulty);
      }
    }
  }

  private IEnumerator endGame() {
    _gameState = GameState.Ended;

    yield return new WaitForSeconds(AIWait);

    canvas.SetActive(true);
    endMenu.SetActive(true);
    // _endMenu.showEndgame(_winner, startingPlayer);

    initializeGame();
  }

  public void doPlayerMove(Square square) {
    // Do not accept player moves when it's the AI's turn
    if(AIPlayers.Contains(_currentPlayer)) {
      return;
    }

    // Player Move
    int index = _boardSquares.IndexOf(square);
    doMove(index);
    if(!_boardState.Contains(Symbol.None)) { // No more squares available
      StartCoroutine(endGame());
    }
  }

  public IEnumerator doAIMove() {
    // If current player is associated with an AI algorithm
    int aiIndex = AIPlayers.IndexOf(_currentPlayer);
    if(aiIndex > -1 && AIBehaviors[aiIndex] != AIType.Human) {
      // Wait AI timer, so ai moves are not instantaneous
      yield return new WaitForSeconds(AIWait);

      if(AIBehaviors[aiIndex].getInstance().nextMove(_currentPlayer, _boardState, out int index)) {
        doMove(index);
      } else { // No possible play
        StartCoroutine(endGame());
      }
    }
  }

  public void doMove(int index) {
    /* Do not if any of these is true:
     *
     * Game Not Started or Finished
     * Current player is None (game did not start)
     * Square of desired play is already occupied
     */
    if(_gameState != GameState.InProgress || _currentPlayer == Symbol.None || _boardState[index] != Symbol.None) {
      return;
    }

    _boardState[index] = _currentPlayer;
    _boardSquares[index].currentSymbol = _currentPlayer;

    _winner = checkWinner(_boardState);

    if(_winner != Symbol.None) {
      StartCoroutine(endGame());
      return;
    }

    // Prompt next player
    _currentPlayer = _currentPlayer.other();

    // Do next AI move, if necessary
    StartCoroutine(doAIMove());
  }

  public static Symbol checkWinner(List<Symbol> board) {
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

}
