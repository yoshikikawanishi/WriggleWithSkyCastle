using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour {


    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag" || collision.tag == "PlayerAttackTag") {
            //セーブ
            DataManager.Instance.Save_Player_Data(transform.position);
            //エフェクト
            GetComponent<ParticleSystem>().Play();
            GetComponent<AudioSource>().Play();
        }
    }
}
