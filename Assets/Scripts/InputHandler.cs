using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class InputHandler : MonoBehaviour {

    private AudioSource _clickSound;

    void Start() {
        _clickSound = GetComponent<AudioSource>();
    }

    void Update() {
        if(Input.GetMouseButtonDown(0)) {
            _clickSound.Play();
        }
    }
}
