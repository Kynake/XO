using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundArt : MonoBehaviour {

  // Slide Vars
  public float slideSpeed;
  public float slideRange;
  public float slideRest;

  private float _slideRestingTime;

  private Vector3 _initialPosition;
  private Vector3 _slideTarget;

  // Sway Vars
  public float swayAngle;
  public float swaySpeed;
  public float swayRest;

  private float _swayRestingTimeX;
  private float _swayRestingTimeZ;

  private float _currentAngleX;
  private float _currentAngleZ;

  private float _swayTargetX;
  private float _swayTargetZ;

  // Generic
  private Vector3 _tempVectorVar;

  void Awake() {

  }

  void Start() {
    // Slide
    _initialPosition = transform.position;
    _slideRestingTime = 0;
    randomSlideTarget();

    // Sway
    _swayRestingTimeX = 0;
    _swayRestingTimeZ = 0;

    _currentAngleX = 0;
    _currentAngleZ = 0;

    _swayTargetX = randomSwayTarget();
    _swayTargetZ = randomSwayTarget();
  }

  void Update() {
    // Slide
    if(transform.position == _slideTarget) { //Rest
      _slideRestingTime += Time.deltaTime;
      if(_slideRestingTime > slideRest) {
        randomSlideTarget();
      }
    } else {
      moveToTarget(_slideTarget);
      _slideRestingTime = 0;
    }

    // Sway
    _tempVectorVar = Vector3.zero;

    if(_currentAngleX == _swayTargetX) {
      restOnSway(ref _swayRestingTimeX, ref _swayTargetX);
    } else {
      _tempVectorVar.x = rotateToAngle(_swayTargetX, ref _currentAngleX);
      _swayRestingTimeX = 0;
    }

    if(_currentAngleZ == _swayTargetZ) {
      restOnSway(ref _swayRestingTimeZ, ref _swayTargetZ);
    } else {
      _tempVectorVar.z = rotateToAngle(_swayTargetZ, ref _currentAngleZ);
      _swayRestingTimeZ = 0;
    }

    transform.Rotate(_tempVectorVar);
  }

  // Slide Methods
  private void randomSlideTarget() {
    _tempVectorVar = Random.onUnitSphere;
    _tempVectorVar.z = 0;

    _slideTarget = (_initialPosition + _tempVectorVar) * slideRange;
  }

  private void moveToTarget(Vector3 target) {
    _tempVectorVar = target - transform.position;
    float stepLength = Mathf.Min(_tempVectorVar.magnitude, slideSpeed * Time.deltaTime);
    _tempVectorVar.Normalize();
    transform.position += _tempVectorVar * stepLength;
  }

  // Sway Methods
  private float randomSwayTarget() {
    return Random.Range(-swayAngle, swayAngle);
  }

  private float rotateToAngle(float swayTarget, ref float currentAngle) {
    bool backRotation = swayTarget < currentAngle; // Rotation angle is negative
    float rotation = Mathf.Min(Mathf.Abs(swayTarget - currentAngle), Time.deltaTime * swaySpeed);
    if(backRotation) {
      rotation = -rotation;
    }

    currentAngle += rotation;
    return rotation;
  }

  private void restOnSway(ref float swayRestingTime, ref float swayTarget) {
    swayRestingTime += Time.deltaTime;
    if(swayRestingTime > swayRest) {
      swayTarget = randomSwayTarget();
    }
  }
}
