using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour {
  public delegate void OnClickDelegate();
  public OnClickDelegate OnClick;

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
    _squareArt = GetComponentInChildren<SquareArt>();
  }

  private void Start() {
    currentSymbol = Symbol.None;
  }

  private void OnMouseDown() => OnClick?.Invoke();
}
