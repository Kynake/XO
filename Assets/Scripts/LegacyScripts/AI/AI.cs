using System.Collections;
using System.Collections.Generic;

public interface AI {
  bool nextMove(Symbol currentPlayer, List<Symbol> board, out int index);
}

public enum AIType {
  Human,
  Easy,
  Medium,
  Hard
}

public static class AITypeExtensions {
  private static AI _easy = null;
  private static AI _medium = null;
  private static AI _hard = null;

  public static AI getInstance(this AIType type) {
    switch(type) {
      case AIType.Easy:
      if(_easy == null) {
          _easy = new RandomAI();
        }
        return _easy;

      case AIType.Medium:
      if(_medium == null) {
          _medium = new RandomMinimaxAI();
        }
        return _medium;

      case AIType.Hard:
        if(_hard == null) {
          _hard = new MinimaxAI();
        }
        return _hard;

      case AIType.Human:
      default:
        return null;
    }
  }
}