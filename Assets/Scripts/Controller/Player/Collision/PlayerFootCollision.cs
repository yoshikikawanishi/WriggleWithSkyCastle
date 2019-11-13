using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootCollision : MonoBehaviour {

    //本体
    private GameObject player;
    //コンポーネント
    private PlayerController player_Controller;
    private AudioSource landing_Sound;

    
    private void Start() {
        player = transform.parent.gameObject;        
        player_Controller   = player.GetComponent<PlayerController>();
    }

    
    protected void OnTriggerStay2D(Collider2D collision) {
        //着地判定
        if (player_Controller.Get_Is_Ride_Beetle()) {
            return;
        }
        if (player_Controller.is_Landing) {
            return;
        }
        foreach (string tag_Name in TagManager.LAND_TAG_LIST) {            
            if (collision.tag == tag_Name) {
                player_Controller.is_Landing = true;
                Landing();                
            }
        }        
    }

    
    private void OnTriggerExit2D(Collider2D collision) {
        //地面から離れる
        foreach(string tag_Name in TagManager.LAND_TAG_LIST) {
            if(collision.tag == tag_Name) {
                player_Controller.is_Landing = false;
            }
        }        
    }


    private void Landing() {
        player_Controller.Change_Animation("IdleBool");
        //landing_Sound.Play();
    }

}
