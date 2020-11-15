using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour {


  private Symbol _currentSymbol;
  private Board _parentBoard;
  private SquareArt _squareArt;

  public Symbol currentSymbol {
    set {
      _squareArt.currentSymbol = value;
      _currentSymbol = value;
    }
    get {
      return _currentSymbol;
    }
  }

  private void Awake() {
    _parentBoard = GetComponentInParent<Board>();
    _squareArt = GetComponentInChildren<SquareArt>();
  }

  private void OnMouseDown() {
    _parentBoard.doPlayerMove(this);
  }
}
