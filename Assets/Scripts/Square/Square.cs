using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour {
  private Board _parentBoard;
  private SquareArt _squareArt;

  private Symbol _currentSymbol;
  public Symbol currentSymbol {
    get {
      return _currentSymbol;
    }
    set {
      if(value != Symbol.None && value == _currentSymbol) {
        return;
      }
      _currentSymbol = value;
      _squareArt.currentSymbol = currentSymbol;
    }
  }

  private void Awake() {
    _parentBoard = GetComponentInParent<Board>();
    _squareArt = GetComponentInChildren<SquareArt>();
  }

  private void Start() {
    currentSymbol = Symbol.None;
  }

  private void OnMouseDown() {
    print("Click");

    // TODO make this into a delegate that the board subscribes to
    // Run Board function

    _parentBoard.clickSquare(this);

  }
}
