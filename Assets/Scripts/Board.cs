using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.Messaging;

public class Board : NetBehaviour {
  private class BoardState {
    private List<Square> _boardSquares;

    public BoardState(Square[] squares, Symbol symbolFill) {
      _boardSquares = new List<Square>(squares);

      for(int i = 0; i < _boardSquares.Count; i++) {
        _boardSquares[i].currentSymbol = symbolFill;
      }
    }

    public int IndexOf(Square square) => _boardSquares.IndexOf(square);
    public void fillBoard(Symbol fill) => _boardSquares.ForEach(square => square.currentSymbol = fill);

    public Symbol this[int i] {
      get => _boardSquares[i].currentSymbol;
      set {
        _boardSquares[i].currentSymbol = value;
      }

    }
  }

  private BoardState _boardState;

  private void Start() {
    _boardState = new BoardState(GetComponentsInChildren<Square>(), Symbol.None);
  }

  public void cycleSymbol(Square square) {
    if(IsServer) {
      cycleSymbol(_boardState.IndexOf(square));
    } else {
      RequestCycleSymbol_ServerRpc(_boardState.IndexOf(square));
    }
  }

  private void cycleSymbol(int index) {
    if(!IsServer) {
      return;
    }

    var newSymbol = _boardState[index] == Symbol.None? Symbol.Cross : _boardState[index].other();

    // Update Server BoardState
    _boardState[index] = newSymbol;

    // Propagate update to clients
    ResponseCycleSymbol_ClientRpc(index, newSymbol);
  }

  [ServerRpc(RequireOwnership = false)]
  private void RequestCycleSymbol_ServerRpc(int index) => cycleSymbol(index);

  [ClientRpc]
  private void ResponseCycleSymbol_ClientRpc(int index, Symbol symbol) {
    _boardState[index] = symbol;
  }
}
