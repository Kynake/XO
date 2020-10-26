using System.Collections;
using System.Collections.Generic;
using Enum = System.Enum;
using UnityEngine;

public class Test : MonoBehaviour {

    public GameObject board;

    private void OnMouseDown() {
        Board boardScript = board.GetComponent<Board>();
        boardScript.boardState = _randomSymbolList();
        Debug.Log(boardScript.boardState);
    }

    private Symbol _randomSymbol() {
        return (Symbol) Random.Range(0, Enum.GetNames(typeof(Symbol)).Length);
    }

    private List<Symbol> _randomSymbolList() {
        List<Symbol> res = new List<Symbol>();
        for (int i = 0; i < 9; i++) {
            res.Add(_randomSymbol());
        }
        return res;
    }
}
