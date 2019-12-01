using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumiaTalk : TalkCharacter {

    protected override IEnumerator Talk() {
        //通常会話時向きを変える
        if(start_ID == 2)
            transform.parent.localScale = new Vector3(-1, 1, 1);
        return base.Talk();
    }


    protected override void Action_In_End_Talk() {
        transform.parent.localScale = new Vector3(1, 1, 1);
        //セリフの変更
        if (start_ID == 2) {
            Change_Message_Status("RumiaText", 5, 5);
        }
    }
}
