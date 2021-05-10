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

    Board.OnGameEnded += (Symbol winner) => revealEndgame(false, winner);
    Board.OnGameInterrupted += () => revealEndgame(true, Symbol.None);

    // Start Disabled
    gameObject.SetActive(false);
  }

  private IEnumerator showEndgame(bool isInterrupted, Symbol winner) {
    yield return new WaitForSeconds(2);

    if(!isInterrupted) {
      if(winner == Symbol.None) {
        _endgameDisplay.text = "Draw";
      } else {
        _endgameDisplay.text = $"{winner} Wins!";
      }
    } else {
      _endgameDisplay.text = "Game Interrupted";
    }

  }

  private void revealEndgame(bool isInterrupted, Symbol winner) {
    MenuController.toggleMenu(gameObject);
    StartCoroutine(showEndgame(isInterrupted, winner));
  }

  // Button Actions
  public void returnToMainMenu() => OnMainMenuReturn?.Invoke();
}
