using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConnectionMenu : MonoBehaviour {
  // Events
  public delegate void OnStartGameDelegate(bool isHost, NetworkTransportTypes transportTypes, string address);
  public static OnStartGameDelegate OnStartGame;

  public delegate void OnMultiplayerMenuDelegate();
  public static OnMultiplayerMenuDelegate OnMultiplayerMenuReturn;

  private bool _isHost;

  [SerializeField]
  private TMP_Text _connectionText;
  private const string _hostText = "Host";
  private const string _clientText = "Join";

  [SerializeField]
  private TMP_InputField _addressInput;

  [SerializeField]
  private TMP_Dropdown _transportDropdown;

  private void Start() {

    MultiplayerMenu.OnClickClient += () => openConnectionMenu(false);
    MultiplayerMenu.OnClickHost += () => openConnectionMenu(true);

    // Start Disabled
    gameObject.SetActive(false);
  }

  // Menu Setups
  private void openConnectionMenu(bool isHost) {
    _isHost = isHost;
    _connectionText.text = $"{(isHost? _hostText : _clientText)} Game:";

    MenuController.toggleMenu(gameObject);
    onChangeTransport();
  }

  // Button Actions
  public void startGame() {
    if(OnStartGame == null) {
      return;
    }

    // Gather Info
    var isHost = true;
    var transport = NetworkTransportTypes.Direct;
    var address = "127.0.0.1";

    // Call Event
    OnStartGame(isHost, transport, address);
  }

  public void onChangeTransport() {
    _addressInput.interactable = !(_transportDropdown.value == 0 && _isHost);
  }

  public void returnToMultiplayerMenu() => OnMultiplayerMenuReturn?.Invoke();
}
