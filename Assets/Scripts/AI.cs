using System.Collections;
using System.Collections.Generic;

public interface AI {
  bool nextMove(Symbol currentPlayer, List<Symbol> board, out int index);
}

public class TestAI : AI {
  public bool nextMove(Symbol currentPlayer, List<Symbol> board, out int index) {
    index = -1;
    for(int i = 0; i < board.Count; i++) {
      if(board[i] == Symbol.None) {
        index = i;
        return true;
      }
    }

    return false;
  }

}
