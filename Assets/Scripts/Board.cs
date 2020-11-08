using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

  // Game Logic
  public Symbol startingPlayer = Symbol.None;

  private Symbol _winner;
  private Symbol _currentPlayer;
  private List<Symbol> _boardState;
  private List<Square> _boardSquares;

  // AI Stuff
  public float AIWait = 0;
  public List<Symbol> AIPlayers = new List<Symbol>();

  private AI _AIBehaviour;

  private void Awake() {
    // Initialize Squares and Game Objects
    _boardState = new List<Symbol>();
    _boardSquares = new List<Square>(GetComponentsInChildren<Square>());
    _AIBehaviour = new TestAI();

    foreach (var square in _boardSquares) {
        _boardState.Add(Symbol.None);
        square.currentSymbol = Symbol.None;
    }

    float boardSize = Mathf.Sqrt(_boardState.Count);
    if(boardSize != (int) boardSize){
        print($"Not a Square Board: {_boardState.Count}");
    }

    // Initialize game
    _currentPlayer = startingPlayer;
    _winner = Symbol.None;

    // Do first AI move, if necessary
    StartCoroutine(doAIMove());
  }

  public void doPlayerMove(Square square) {
    // Do not accept player moves when it's the AI's turn
    if(AIPlayers.Contains(_currentPlayer)) {
        return;
    }

    // Player Move
    int index = _boardSquares.IndexOf(square);
    doMove(index);
  }

  public IEnumerator doAIMove() {
    // Wait AI timer, so ai moves are not instantaneous
    yield return new WaitForSeconds(AIWait);

    // If current player is associated with an AI algorithm
    if(AIPlayers.Contains(_currentPlayer)) {
        int AIPos = _AIBehaviour.nextMove(_currentPlayer, _boardState);
        doMove(AIPos);
    }
  }

  public void doMove(int index) {
    /* Do not if any of these is true:
     *
     * Game has been won
     * Current player is None (game did not start)
     * Index of Square to play is invalid
     * Square of desired play is already occupied
     */
    if(_winner != Symbol.None || _currentPlayer == Symbol.None || index < 0 || _boardState[index] != Symbol.None) {
        return;
    }

    _boardState[index] = _currentPlayer;
    _boardSquares[index].currentSymbol = _currentPlayer;

    _winner = checkWinner(_boardState);
    string winnerText;
    switch (_winner) {
        case Symbol.Cross:
            winnerText = "Cross";
            break;

        case Symbol.Nought:
            winnerText = "Nought";
            break;

        default:
            winnerText = "None";
            break;
    }
    print($"Winner: {winnerText}");

    if(_winner != Symbol.None) {
        return;
    }

    // Prompt next player
    _currentPlayer = _currentPlayer.other();

    // Do next AI move, if necessary
    StartCoroutine(doAIMove());
  }

  public Symbol checkWinner(List<Symbol> board) {
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
