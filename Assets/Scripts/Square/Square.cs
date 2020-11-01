using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour {
    public GameObject crossMesh;
    public GameObject noughtMesh;

    private Symbol _currentSymbol;

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

    private void Start() {
    }

    private void OnMouseDown() {
        GetComponentInParent<Board>().squareClicked(this);
    }
}
