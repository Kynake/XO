using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMinimaxAI : AI {
  public float randomChance;

  private AI _minimax = new MinimaxAI();
  private AI _random = new RandomAI();
  private const float _defaultChance = 50f;

  public RandomMinimaxAI() {
    randomChance = _defaultChance;
  }

  public RandomMinimaxAI(float chance) {
    randomChance = Mathf.Clamp(chance, 0f, 100f);
  }

  public bool nextMove(Symbol currentPlayer, List<Symbol> board, out int index) {
    return Random.Range(0f, 100f) <= randomChance
      ? _random.nextMove(currentPlayer, board, out index)
      : _minimax.nextMove(currentPlayer, board, out index);
  }
}