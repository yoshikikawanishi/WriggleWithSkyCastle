using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemunoSoundEffect : MonoBehaviour {

    [SerializeField] private AudioSource before_Slash_Sound;
    [SerializeField] private AudioSource slash_Sound;


    public void Play_Before_Slash_Sound() {
        before_Slash_Sound.Play();
    }

    public void Play_Slash_Sound() {
        slash_Sound.Play();
    }
}
