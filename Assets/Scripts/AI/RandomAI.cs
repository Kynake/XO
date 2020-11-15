using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAI : AI {
  public bool nextMove(Symbol currentPlayer, List<Symbol> board, out int index) {
    List<int> indexList = new List<int>(board.Count);
		for(int i = 0; i < board.Count; i++) {
			if(board[i] == Symbol.None) {
				indexList.Add(i);
			}
		}

		if(indexList.Count > 0) {
			index = indexList[Random.Range(0, indexList.Count)];
			return true;
		}

		index = -1;
		return false;
  }
}
