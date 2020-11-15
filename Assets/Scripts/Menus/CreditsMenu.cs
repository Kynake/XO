using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsMenu : MonoBehaviour {

  public TextAsset credits;

  void Awake() {
    Text creditsDisplay = gameObject.GetComponentInChildren<Text>();
    creditsDisplay.text = credits.text;
  }

}
