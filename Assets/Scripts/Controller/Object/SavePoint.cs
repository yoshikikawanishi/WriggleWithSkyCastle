using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour {

    [SerializeField] private string canvas_Name = "Canvas";

    private List<string> collide_Tags = new List<string> {
        "PlayerBodyTag",
        "PlayerAttackTag",
        "PlayerButterflyAttackTag",
        "PlayerSpiderAttackTag"
    };

    private void OnTriggerEnter2D(Collider2D collision) {
        foreach (string tag in collide_Tags) {
            if (collision.tag == tag) {
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
}
