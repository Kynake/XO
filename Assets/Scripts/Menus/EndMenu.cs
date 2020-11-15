using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndMenu : MonoBehaviour {

  public GameObject canvas;
  public GameObject winnerText;

  private Text _winner;

  void Awake() {
    _winner = winnerText.GetComponent<Text>();
  }

  public void showEndgame(Symbol winner, Symbol startingPlayer) {
    gameObject.SetActive(true);

    if(winner == Symbol.None) {
      _winner.text = "Draw";
    } else {
      _winner.text = $"Player {(winner == startingPlayer? 1 : 2)} Wins!";
    }

    canvas.SetActive(true);
  }
}
