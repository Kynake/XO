using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {
  public List<AudioClip> clickSounds = new List<AudioClip>();
  private AudioSource _clickPlayer;

  void Start() {
    _clickPlayer = GetComponent<AudioSource>();
  }

  void Update() {
    if(Input.GetMouseButtonDown(0)) {
      if(clickSounds.Capacity > 0) {
        _clickPlayer.clip = clickSounds[Random.Range(0, clickSounds.Capacity)];
        _clickPlayer.Play();
      }
    }
  }
}
