﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    public Symbol startingPlayer = Symbol.None;
    public List<Symbol> AIPlayers = new List<Symbol>();

    private Symbol _currentPlayer;
    private List<Symbol> _boardState;
    private List<Square> _boardSquares;

    private Symbol _winner;

    private void Awake() {
        // Initialize Squares and Game Objects
        _boardState = new List<Symbol>();
        _boardSquares = new List<Square>(GetComponentsInChildren<Square>());

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
        doAIMove();
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

    public void doAIMove() {
        if(AIPlayers.Contains(_currentPlayer)) {
            // int AIPos = minimax();
            // doMove(AIPos);
        }
    }

    public void doMove(int index) {
        if(_winner != Symbol.None || _boardState[index] != Symbol.None || _currentPlayer == Symbol.None) {
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

        // Prompt next player
        _currentPlayer = _currentPlayer.other();

        // Do next AI move, if necessary
        doAIMove();
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
