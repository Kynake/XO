using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardScript : MonoBehaviour {

    public void squareClicked(SquareScript square) {
        switch(square.CurrentSymbol) {
            case Symbol.None:
                square.CurrentSymbol = Symbol.Cross;
                break;

            case Symbol.Cross:
                square.CurrentSymbol = Symbol.Nought;
                break;

            case Symbol.Nought:
                square.CurrentSymbol = Symbol.None;
                break;
        }
    }
}
