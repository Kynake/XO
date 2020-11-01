using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    public Symbol currentPlayer = Symbol.None;

    private List<Symbol> _boardState;
    private List<Square> _boardSquares;

    public void squareClicked(Square square) {
        Debug.Log($"Square clicked: {square.currentSymbol}");

        if(square.currentSymbol != Symbol.None) {
            return;
        }

        int index = _boardSquares.IndexOf(square);

        _boardState[index] = currentPlayer;
        square.currentSymbol = currentPlayer;

        currentPlayer = SymbolOperations.other(currentPlayer);
    }

    public List<Symbol> boardState {
        get {
            return _boardState;
        }
        set {
            _boardState = value;
            for (int i = 0; i < _boardState.Count; i++) {
                if(_boardState[i] != _boardSquares[i].currentSymbol) {
                    _boardSquares[i].currentSymbol = _boardState[i];
                }
            }
        }
    }

    private void Start() {
        _boardState = new List<Symbol>();
        _boardSquares = new List<Square>(GetComponentsInChildren<Square>());

        foreach (var square in _boardSquares) {
            _boardState.Add(Symbol.None);
            square.currentSymbol = Symbol.None;
        }
    }

}
