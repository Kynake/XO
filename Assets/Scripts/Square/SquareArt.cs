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
  // Prefab mesh references
  public GameObject crossMesh;
  public GameObject noughtMesh;
  public GameObject globeMesh;

  public Symbol currentSymbol {
    set {
      crossMesh.SetActive(value == Symbol.Cross);
      noughtMesh.SetActive(value == Symbol.Nought);
      globeMesh.SetActive(value != Symbol.Cross);

      AudioClip sfx = null;
      switch(value) {
        case Symbol.Cross:
          sfx = explosion;
          break;

        case Symbol.Nought:
          sfx = shield;
          break;
      }

      if(sfx != null) {
        _audioPlayer.PlayOneShot(sfx);
      }
    }
  }

  // Audio asset references
  public AudioClip explosion; // X Effect
  public AudioClip shield;    // O Effect

  private AudioSource _audioPlayer;

  // Vector Movement Stuff
  public float crossSpinRate = 30f;
  public float noughtSpinRate = 30f;
  public float globeSpinRate = 7.5f;

  public float bobbingFrequency = 1;
  public float bobbingAmplitude = 0.15f;

  private float _bobbingCycleTimeElapsed;
  private Vector3 _previousBobbingAmount;

  // Holding vars
  private Vector3 _newBobbingAmount;
  private Vector3 _rotationVector;

  void Awake() {
    _audioPlayer = GetComponent<AudioSource>();
  }

  void Start() {

    _bobbingCycleTimeElapsed = Random.Range(0f, 2*Mathf.PI);
    // transform.Rotate(new Vector3(0, Random.Range(0f, 360f), 0)); // Uncomment to start with random rotations

    // _bobbingCycleTimeElapsed = 0; // Uncomment to start all with the same heigth
    _previousBobbingAmount = Vector3.zero;

    // Initialize holding vars
    _newBobbingAmount = Vector3.zero;
    _rotationVector = Vector3.zero;
  }

  void Update() {
    // Do movement applied to all meshes
    _bobbingCycleTimeElapsed += Time.deltaTime % (2 * Mathf.PI);
    _newBobbingAmount.Set(0, Mathf.Sin(_bobbingCycleTimeElapsed * (2 * Mathf.PI / bobbingFrequency)) * bobbingAmplitude, 0);

    transform.position += (_newBobbingAmount - _previousBobbingAmount);
    _previousBobbingAmount = _newBobbingAmount;

    // Do each mesh movement
    crossMovement();
    noughtMovement();
    globeMovement();
  }

  private void crossMovement() {
    meshMovement(crossMesh, crossSpinRate);
  }

  private void noughtMovement() {
    meshMovement(noughtMesh, noughtSpinRate);
  }

  private void globeMovement() {
    meshMovement(globeMesh, globeSpinRate);
  }

  private void meshMovement(GameObject mesh, float meshSpinRate) {
    _rotationVector.Set(0, meshSpinRate * Time.deltaTime, 0);
    mesh.transform.Rotate(_rotationVector);
  }
}
