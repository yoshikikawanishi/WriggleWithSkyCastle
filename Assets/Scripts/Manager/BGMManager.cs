using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : SingletonMonoBehaviour<BGMManager> {

    //書き換えないこと
    //Inspecterで代入して、外から取得したい
    public AudioClip stage2_Boss_BGM;

    private AudioClip now_BGM;
    private AudioSource audio_Source;


	// Use this for initialization
	void Start () {
        audio_Source = GetComponent<AudioSource>();        
	}
	

    public void Change_BGM(AudioClip next_BGM) {
        if(now_BGM != next_BGM) {
            audio_Source.clip = next_BGM;
            audio_Source.Play();
            now_BGM = next_BGM;
        }
    }

    public void Change_BGM(AudioClip next_BGM, float volume) {
        if (now_BGM != next_BGM) {
            audio_Source.PlayOneShot(next_BGM);
            audio_Source.volume = volume;
            now_BGM = next_BGM;
        }
    }

    public void Stop_BGM() {
        audio_Source.Stop();
    }
    
}
