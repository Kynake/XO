using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour {

  public List<GameObject> playerOneOptions;
  public List<GameObject> playerTwoOptions;
  public GameObject board;

  private List<Toggle> _playerOneToggles;
  private List<Toggle> _playerTwoToggles;

  private bool _isSwitching = false;

  private Canvas _canvas;

  void Awake() {
    _canvas = GetComponentInParent<Canvas>();


    _playerOneToggles = playerOneOptions.ConvertAll<Toggle>(button => button.GetComponent<Toggle>());
    _playerTwoToggles = playerTwoOptions.ConvertAll<Toggle>(button => button.GetComponent<Toggle>());
  }

  public void startGame() {
    int count = 0;
    _playerOneToggles.ForEach(toggle => count += toggle.isOn? 1 : 0);
    _playerTwoToggles.ForEach(toggle => count += toggle.isOn? 1 : 0);
    if(count != 2) {
      return;
    }

    _canvas.gameObject.SetActive(false);
    gameObject.SetActive(false);
    board.GetComponent<BoardLegacy>().startGame();
  }

  public void playerOneToggle(GameObject button) {
    playerToggle(button, _playerOneToggles);
    AIType difficulty;
    switch(button.tag) {
      case "Easy":
        difficulty = AIType.Easy;
      break;

      case "Medium":
        difficulty = AIType.Medium;
      break;

      case "Hard":
        difficulty = AIType.Hard;
      break;

      case "Human": default:
        difficulty = AIType.Human;
        break;
    }
    board.GetComponent<BoardLegacy>().setPlayerOneAI(difficulty);
  }

  public void playerTwoToggle(GameObject button) {
    playerToggle(button, _playerTwoToggles);
    AIType difficulty;
    switch(button.tag) {
      case "Easy":
        difficulty = AIType.Easy;
      break;

      case "Medium":
        difficulty = AIType.Medium;
      break;

      case "Hard":
        difficulty = AIType.Hard;
      break;

      case "Human": default:
        difficulty = AIType.Human;
        break;
    }
    board.GetComponent<BoardLegacy>().setPlayerTwoAI(difficulty);
  }

  private void playerToggle(GameObject button, List<Toggle> toggles) {
    if(_isSwitching) {
      return;
    }
    _isSwitching = true;

    Toggle buttonToggle = button.GetComponent<Toggle>();
    toggles.ForEach(toggle => toggle.isOn = false);
    buttonToggle.isOn = true;

    _isSwitching = false;
  }
}
