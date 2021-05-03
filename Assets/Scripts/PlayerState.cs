using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

public class PlayerState : NetBehaviour {
  [SerializeField]
  private NetworkVariable<Symbol> _playerSymbol = new NetworkVariable<Symbol>(new NetworkVariableSettings {
    WritePermission = NetworkVariablePermission.ServerOnly,
    ReadPermission = NetworkVariablePermission.Everyone
  });

  public Symbol playerSymbol {
    get {
      return _playerSymbol.Value;
    }
    set {
      if(!IsServer) {
        return;
      }

      _playerSymbol.Value = value;
    }
  }
}
