using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Symbol {
    None,
    Cross,
    Nought
}

public class Square : MonoBehaviour {
    public int line;
    public int column;

    public GameObject crossMesh;
    public GameObject noughtMesh;

    public Symbol CurrentSymbol {
        set {
            crossMesh.SetActive(value == Symbol.Cross);
            noughtMesh.SetActive(value == Symbol.Nought);
        }
        get {
            if(crossMesh.activeSelf) {
                return Symbol.Cross;
            }
            if(noughtMesh.activeSelf) {
                return Symbol.Nought;
            }
            return Symbol.None;
        }
    }

    void Start() {
        CurrentSymbol = Symbol.None;
    }

    private void OnMouseDown() {
        GetComponentInParent<BoardScript>().squareClicked(this);
    }
}
