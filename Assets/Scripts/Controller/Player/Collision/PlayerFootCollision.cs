using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootCollision : MonoBehaviour {

    //本体
    private GameObject player;
    //コンポーネント
    private PlayerController player_Controller;
    private PlayerSoundEffect player_SE;
    private Rigidbody2D player_Rigid;

    //着地判定を取るタグリスト
    private List<string> LANDING_TAG_LIST = new List<string> {
        "GroundTag",
        "ThroughGroundTag",
        "SandbackGroundTag",
        "DamagedGroundTag"
    };

    
    private void Start() {
        player = transform.parent.gameObject;        
        player_Controller   = player.GetComponent<PlayerController>();
        player_SE = player.GetComponentInChildren<PlayerSoundEffect>();
        player_Rigid = player.GetComponent<Rigidbody2D>();
    }

    
    protected void OnTriggerStay2D(Collider2D collision) {
        
        if (player_Controller.Get_Is_Ride_Beetle()) //飛行中は無し 
            return;        
        if (player_Controller.is_Landing)           //すでに地面にいるとき
            return;
        if (player_Rigid.velocity.y > 10f)          //上昇中
            return; 

        //着地判定
        foreach (string tag_Name in LANDING_TAG_LIST) {            
            if (collision.tag == tag_Name) {
                player_Controller.is_Landing = true;
                Landing();                
            }
        }        
    }

    
    private void OnTriggerExit2D(Collider2D collision) {
        //地面から離れる
        foreach(string tag_Name in LANDING_TAG_LIST) {
            if(collision.tag == tag_Name) {
                player_Controller.is_Landing = false;
            }
        }        
    }


    private void Landing() {
        player_Controller.Change_Animation("IdleBool");
        player_SE.Play_Land_Sound();
    }

}
