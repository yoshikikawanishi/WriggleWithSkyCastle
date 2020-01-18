using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsualSoundManager : SingletonMonoBehaviour<UsualSoundManager> {


    [SerializeField] private AudioSource get_Small_Item_Sound;
    [SerializeField] private AudioSource life_Up_Sound;
    [SerializeField] private AudioSource stock_Up_Sound;
    [Space]
    [SerializeField] private AudioSource enemy_Shoot_Sound;
    [SerializeField] private AudioSource enemy_Shoot_Sound_Small;
    [SerializeField] private AudioSource enemy_Attack_Sound;


    public void Play_Get_Small_Item_Sound() {
        get_Small_Item_Sound.Play();
    }

    public void Play_Life_Up_Sound() {
        life_Up_Sound.Play();
    }

    public void Play_Stock_Up_Sound() {
        stock_Up_Sound.Play();
    }

    public void Play_Shoot_Sound() {
        enemy_Shoot_Sound.volume = 0.1f;
        enemy_Shoot_Sound.Play();
    }

    public void Play_Shoot_Sound(float volume) {
        enemy_Shoot_Sound.volume = volume;
        enemy_Shoot_Sound.Play();
    }

    public void Play_Shoot_Sound_Small() {
        enemy_Shoot_Sound_Small.Play();
    }

    public void Play_Attack_Sound() {
        enemy_Attack_Sound.Play();
    }

}
