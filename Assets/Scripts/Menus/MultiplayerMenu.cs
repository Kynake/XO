using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerMenu : MonoBehaviour {
  // Events
  public delegate void OnClickClientDelegate();
  public static OnClickClientDelegate OnClickClient;

  public delegate void OnClickHostDelegate();
  public static OnClickHostDelegate OnClickHost;

  public delegate void OnMainMenuDelegate();
  public static OnMainMenuDelegate OnMainMenuReturn;

  private void Start() {
    MainMenu.OnClickStart += () => MenuController.toggleMenu(gameObject);

    // Start Disabled
    gameObject.SetActive(false);
  }

  // Button Actions
  public void selectClient() => OnClickClient?.Invoke();
  public void selectHost() => OnClickHost?.Invoke();
  public void returnToMainMenu() => OnMainMenuReturn?.Invoke();
}
