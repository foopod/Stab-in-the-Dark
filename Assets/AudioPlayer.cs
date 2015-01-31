using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour {
    void Start() {
        audio.Play();
        audio.Play(44100);
    }
}