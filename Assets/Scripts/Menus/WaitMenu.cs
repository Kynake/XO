using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaitMenu : MonoBehaviour {
  // Events
  public delegate void CancelDelegate();
  public static CancelDelegate OnCancel;

  private const string _hostText = "Waiting for player 2...";
  private const string _clientText = "Joining game...";

  [SerializeField]
  private TMP_Text _connectingText = null;

  private void Start() {
    ConnectionMenu.OnStartGame += (isHost, transport, address) => openWaitMenu(isHost);

    // Start Disabled
    gameObject.SetActive(false);
  }

  private void openWaitMenu(bool isHost) {
    _connectingText.text = isHost? _hostText : _clientText;

    MenuController.toggleMenu(gameObject);
  }

  public void cancelConnection() => OnCancel?.Invoke();
}
