using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConnectionMenu : MonoBehaviour {
  // Events
  public delegate void OnStartGameDelegate(bool isHost, NetworkTransportTypes transport, string address);
  public static OnStartGameDelegate OnStartGame;

  public delegate void OnMultiplayerMenuDelegate();
  public static OnMultiplayerMenuDelegate OnMultiplayerMenuReturn;

  private bool _isHost;

  [SerializeField]
  private TMP_Text _connectionText = null;
  private const string _hostText = "Host";
  private const string _clientText = "Join";

  [SerializeField]
  private TMP_InputField _addressInput = null;

  [SerializeField]
  private TMP_Text _addressPlaceholder = null;
  private const string _placeholderPrompt = " Enter Address...";
  private const string _placeholderIgnore = " [Ignored]";

  [SerializeField]
  private TMP_Dropdown _transportDropdown = null;

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
    var transport = _transportDropdown.value == 0? NetworkTransportTypes.Direct : NetworkTransportTypes.Relayed;
    var address = _addressInput.text;

    if(address == "" && (!_isHost || transport != NetworkTransportTypes.Direct)) {
      return;
    }

    // Call Event
    OnStartGame(_isHost, transport, address);
  }

  public void onChangeTransport() {
    _addressInput.interactable = !(_transportDropdown.value == 0 && _isHost);
    _addressPlaceholder.text = _addressInput.interactable? _placeholderPrompt : _placeholderIgnore;
  }

  public void returnToMultiplayerMenu() => OnMultiplayerMenuReturn?.Invoke();
}
