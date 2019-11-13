using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour {


    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag") {
            DataManager.Instance.Save_Player_Data(transform.position);
        }
    }
}
