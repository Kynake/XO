using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Symbol {
    None,
    Cross,
    Nought
}

public class Square : MonoBehaviour {
    public GameObject crossMesh;
    public GameObject noughtMesh;

    private Symbol _currentSymbol;

    public Symbol CurrentSymbol {
        set {
            crossMesh.SetActive(value == Symbol.Cross);
            noughtMesh.SetActive(value == Symbol.Nought);
            _currentSymbol = value;
        }
        get {
            return _currentSymbol;
        }
    }

    private void Start() {
        CurrentSymbol = Symbol.Cross;
    }

    private void OnMouseDown() {
        GetComponentInParent<Board>().squareClicked(this);
    }
}
