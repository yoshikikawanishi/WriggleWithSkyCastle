using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rumia : TalkCharacter {


    protected override float Action_Before_Talk() {
        GetComponent<ParticleSystem>().Stop();
        return 0f;
    }

    protected override IEnumerator Talk() {
        //通常会話時向きを変える
        if (start_ID == 2)
            transform.localScale = new Vector3(-1, 1, 1);
        return base.Talk();
    }


    protected override void Action_In_End_Talk() {
        transform.localScale = new Vector3(1, 1, 1);
        Stage1_1Scene.Instance.rumia_State = Stage1_1Scene.Rumia.find;
        if (start_ID == 2) {
            //セリフの変更
            Change_Message_Status("RumiaText", 5, 5);
            //宝箱出す
            Put_Out_Collection_Box();
        }
    }


}
