using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour {

    [SerializeField] private string canvas_Name = "Canvas";

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag" || collision.tag == "PlayerAttackTag") {
            //セーブ
            DataManager.Instance.Save_Player_Data(transform.position);
            //エフェクト
            GetComponent<ParticleSystem>().Play();
            GetComponent<AudioSource>().Play();
            //UIの表示
            GameObject.Find(canvas_Name).GetComponent<GameUIController>().Display_Save_Text();
        }
    }
}
