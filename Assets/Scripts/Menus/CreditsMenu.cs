using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreditsMenu : MonoBehaviour {
  // Events
  public delegate void OnMainMenuDelegate();
  public static OnMainMenuDelegate OnMainMenuReturn;

  public TextAsset credits;

  private void Awake() {
    TextMeshProUGUI creditsDisplay = GetComponentInChildren<TMPro.TextMeshProUGUI>();
    creditsDisplay.text = credits.text;
  }

  private void Start() {
    MainMenu.OnClickCredits += () => MenuController.toggleMenu(gameObject);

    // Start Disabled
    gameObject.SetActive(false);
  }

  // Button Actions
  public void returnToMainMenu() => OnMainMenuReturn?.Invoke();
}
