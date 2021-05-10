using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndMenu : MonoBehaviour {
  // Events
  public delegate void OnMainMenuDelegate();
  public static OnMainMenuDelegate OnMainMenuReturn;

  private TextMeshProUGUI _endgameDisplay;

  void Start() {
    _endgameDisplay = GetComponentInChildren<TextMeshProUGUI>();

    Board.OnGameEnded += (Symbol winner) => showEndgame(false, winner);
    Board.OnGameInterrupted += () => showEndgame(true, Symbol.None);

    // Start Disabled
    gameObject.SetActive(false);
  }

  private void showEndgame(bool isInterrupted, Symbol winner) {
    if(!isInterrupted) {
      if(winner == Symbol.None) {
        _endgameDisplay.text = "Draw";
      } else {
        _endgameDisplay.text = $"{(winner == Symbol.Cross? "Cross" : "Circle")} Wins!";
      }
    } else {
      _endgameDisplay.text = "Game Interrupted";
    }

    MenuController.toggleMenu(gameObject);
  }

  // Button Actions
  public void returnToMainMenu() => OnMainMenuReturn?.Invoke();
}
