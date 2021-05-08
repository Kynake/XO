using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportMenu : MonoBehaviour {
  // Events
  public delegate void OnSelectIPDelegate();
  public static OnSelectIPDelegate OnSelectIP;

  public delegate void OnSelectPhotonDelegate();
  public static OnSelectPhotonDelegate OnSelectPhoton;

  public delegate void OnMainMenuDelegate();
  public static OnMainMenuDelegate OnMainMenuReturn;

  private void Start() {
    MainMenu.OnClickStart += () => MenuController.toggleMenu(gameObject);

    // Start Disabled
    gameObject.SetActive(false);
  }

  // Button Actions
  public void selectIP() => OnSelectIP?.Invoke();
  public void selectPhoton() => OnSelectPhoton?.Invoke();
  public void returnToMainMenu() => OnMainMenuReturn?.Invoke();
}
