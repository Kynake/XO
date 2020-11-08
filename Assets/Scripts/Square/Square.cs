using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour {
    public GameObject crossMesh;
    public GameObject noughtMesh;

    private Symbol _currentSymbol;
    private Board _parentBoard;

    public Symbol currentSymbol {
        set {
            crossMesh.SetActive(value == Symbol.Cross);
            noughtMesh.SetActive(value == Symbol.Nought);
            _currentSymbol = value;
        }
        get {
            return _currentSymbol;
        }
    }

    private void Awake() {
        _parentBoard = GetComponentInParent<Board>();
    }

    private void OnMouseDown() {
        _parentBoard.doPlayerMove(this);
    }
}
