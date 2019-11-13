using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour {

    private float time = 0;

    private List<string> tag_List = new List<string> {
        "GroundTag",
        "ScreenWallTag"
    };

    private void OnTriggerStay2D(Collider2D collision) {
       foreach(string tag in tag_List) {
            if(collision.tag == tag) {
                time += Time.deltaTime;
            }
        }
        if(time >= 0.5f) {
            PlayerManager.Instance.Set_Life(0);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        foreach (string tag in tag_List) {
            if (collision.tag == tag) {
                time = 0;
            }
        }
    }
}
