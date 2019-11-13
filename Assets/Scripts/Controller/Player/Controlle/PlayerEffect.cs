using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour {

    
    [SerializeField] private GameObject[] shoot_Charge = new GameObject[3];

    private ParticleSystem[] shoot_Charge_Particle = new ParticleSystem[3];


    private void Start() {
        //取得
        for (int i = 0; i < 3; i++) {
            shoot_Charge_Particle[i] = shoot_Charge[i].GetComponent<ParticleSystem>();
        }
       
    }

    /// <summary>
    /// チャージショット用エフェクト
    /// </summary>
    /// <param name="phase">チャージ段階(１～３段階)それ以外の場合エフェクト止める</param>
    public void Start_Shoot_Charge(int phase) {        
        for(int i = 0; i < 3; i++) {
            if(i + 1 == phase) {
                shoot_Charge_Particle[i].Play();
            }
            else {
                shoot_Charge_Particle[i].Stop();
            }
        }
    }
}
