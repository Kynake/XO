using System.Collections;
using System.Collections.Generic;

public interface AI {
  int nextMove(Symbol currentPlayer, List<Symbol> board);
}

public class TestAI : AI {
  public int nextMove(Symbol currentPlayer, List<Symbol> board) {
    for(int i = 0; i < board.Count; i++) {
      if(board[i] == Symbol.None) {
        return i;
      }
    }

    return -1;
  }

}
