using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YukaFlower : MonoBehaviour {

    private YukaTalk _talk;

    private List<string> talk_Tags = new List<string> {
        "PlayerAttackTag",
        "PlayerButterflyAttackTag",
        "PlayerSpiderAttackTag",
    };


    // Use this for initialization
    void Start () {
        _talk = transform.parent.GetComponent<YukaTalk>();	
	}


    private void OnTriggerEnter2D(Collider2D collision) {
        foreach(string tag in talk_Tags) {
            if(collision.tag == tag) {
                _talk.Change_Message();
                _talk.Start_Talk();
            }
        }
    }


    


}
