using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndMenu : MonoBehaviour {

  private Canvas _canvas;
  private TextMeshProUGUI _winnerDisplay;

  void Awake() {
    _winnerDisplay = GetComponentInChildren<TextMeshProUGUI>();
    _canvas = GetComponentInParent<Canvas>();
  }

  public void showEndgame(Symbol winner, Symbol startingPlayer) {
    gameObject.SetActive(true);

    if(winner == Symbol.None) {
      _winnerDisplay.text = "Draw";
    } else {
      _winnerDisplay.text = $"Player {(winner == startingPlayer? 1 : 2)} Wins!";
    }

    _canvas.gameObject.SetActive(true);
  }
}
