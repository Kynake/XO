using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    public void squareClicked(Square square) {
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

    public List<Symbol> boardState {
        get {
            List<Square> squares = new List<Square>(GetComponentsInChildren<Square>());
            return squares.ConvertAll<Symbol>(square => square.CurrentSymbol);
        }
        set {
            Square[] squares = GetComponentsInChildren<Square>();
            for (int i = 0; i < squares.Length; i++) {
                if(value[i] != squares[i].CurrentSymbol) {
                    squares[i].CurrentSymbol = value[i];
                }
            }
        }
    }
}
