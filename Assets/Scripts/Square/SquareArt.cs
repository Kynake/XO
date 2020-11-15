using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * SquareArt
 *
 * Code to control behaviour of the visuals of a square
 * without affecting it's logic or bounding box.
 * e.g. Animation
 */
public class SquareArt : MonoBehaviour {
  public GameObject crossMesh;
  public GameObject noughtMesh;
  public GameObject globeMesh;

  public Symbol currentSymbol {
    set {
      crossMesh.SetActive(value == Symbol.Cross);
      noughtMesh.SetActive(value == Symbol.Nought);
      globeMesh.SetActive(value != Symbol.Cross);
    }
  }

  public float spinRate = 60;
  public float bobbingFrequency = 1;
  public float bobbingAmplitude = 0.15f;

  private float _bobbingCycleTimeElapsed;
  private Vector3 _previousBobbingAmount;

  void Awake() {


    // _bobbingCycleTimeElapsed = Random.Range(0f, 2*Mathf.PI);
    _bobbingCycleTimeElapsed = 0;
    _previousBobbingAmount = Vector3.zero;
  }

  void Update() {
    _bobbingCycleTimeElapsed += Time.deltaTime % (2 * Mathf.PI);
    Vector3 newBobbingAmount = new Vector3(0, Mathf.Sin(_bobbingCycleTimeElapsed * (2 * Mathf.PI / bobbingFrequency)) * bobbingAmplitude, 0);

    transform.position += (newBobbingAmount - _previousBobbingAmount);
    _previousBobbingAmount = newBobbingAmount;


    transform.Rotate(new Vector3(0, spinRate * Time.deltaTime, 0));
  }
}
