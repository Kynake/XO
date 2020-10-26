﻿using System.Collections;
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
}
