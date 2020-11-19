using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreditsMenu : MonoBehaviour {

  public TextAsset credits;

  void Awake() {
    TextMeshProUGUI creditsDisplay = GetComponentInChildren<TMPro.TextMeshProUGUI>();
    creditsDisplay.text = credits.text;
  }

}
