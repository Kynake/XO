using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour {

  [SerializeField]
  private GameObject _canvas = null;

  [SerializeField]
  private GameObject _board = null;

  public void startGame() {
    _canvas.SetActive(false);
    _board.GetComponent<Board>().startGame();
  }

}
