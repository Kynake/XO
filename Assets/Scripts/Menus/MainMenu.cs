using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
  // Events
  public delegate void OnClickStartDelegate();
  public static OnClickStartDelegate OnClickStart;

  public delegate void OnClickCreditsDelegate();
  public static OnClickCreditsDelegate OnClickCredits;

  private void Start() {
    CreditsMenu.OnMainMenuReturn += () => MenuController.toggleMenu(gameObject);
  }

  // Button Actions
  public void startGame() => OnClickStart?.Invoke();
  public void viewCredits() => OnClickCredits?.Invoke();
  public void quitGame() => Application.Quit();
}
