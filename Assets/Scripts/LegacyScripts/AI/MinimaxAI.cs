using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimaxAI : AI {
  private struct Move {
    public int index;
    public int score;
  }

  // TODO: guardar boardClone na classe

  public bool nextMove(Symbol currentPlayer, List<Symbol> board, out int index) {
    List<Symbol> boardClone = new List<Symbol>(board);
    bool res = minimaxMove(currentPlayer, boardClone, out var move);

    index = move.index;
    return res;
  }

  private bool minimaxMove(Symbol player, List<Symbol> board, out Move bestMove) {
    bool canPlay = false;
    bool isMin = (int) player < 0; // Nought

    bestMove.index = -1;
    bestMove.score = 100 * -(int) player;

    // Only play recursively if there's no winner
    Symbol winner = BoardLegacy.checkWinner(board);
    if(winner != Symbol.None) {
      bestMove.score = (int) winner;
      return false;
    }

    for(int i = 0; i < board.Count; i++) {
      if(board[i] != Symbol.None) {
        continue;
      }
      canPlay = true;

      board[i] = player;
      minimaxMove(player.other(), board, out var nextMove);
      if((isMin && nextMove.score < bestMove.score) || (!isMin && nextMove.score > bestMove.score)) {
        bestMove.index = i;
        bestMove.score = nextMove.score;
      }

      board[i] = Symbol.None;
    }

    if(!canPlay) {
      bestMove.score = 0;
    }

    return canPlay;
  }
}