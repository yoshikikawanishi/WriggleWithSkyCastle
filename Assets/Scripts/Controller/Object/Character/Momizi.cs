using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Momizi : TalkCharacter {

    [SerializeField] private RopeWay rop_Way;

    protected override float Action_Before_Talk() {
        //ロープウェイが届いたらセリフ変える
        if (start_ID == 1) {
            if (rop_Way.transform.position.x > 2200f && rop_Way.transform.position.y > -16f) {
                Change_Message_Status("MomiziText", 2, 4);
            }
        }
        return 0;
    }


    protected override void Action_In_End_Talk() {
        //ロープウェイ到着感謝後
        if (start_ID == 2) {
            Change_Message_Status("MomiziText", 4, 4);            
            Put_Out_Collection_Box();
        }
    }

}
