using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rumia : TalkCharacter {

    private GameObject player;

    private bool is_Waiting = true;

    private new void Start() {
        base.Start();
        player = GameObject.FindWithTag("PlayerTag");
    }


    private void Update() {
        if (is_Waiting)
            return;
        //常に自機の方を向く
        transform.localScale = new Vector3(Compare_Player_Position(), 1, 1);
        
    }

    //自機が右にいるとき１、左にいるとき-1を返す
    private int Compare_Player_Position() {
        if((player.transform.position.x - transform.position.x).CompareTo(0) < 0) {
            return -1;
        }
        return 1;
    }


    protected override float Action_Before_Talk() {
        GetComponent<ParticleSystem>().Stop();
        is_Waiting = false;
        return 0f;
    }    


    protected override void Action_In_End_Talk() {
        Stage1_1Scene.Instance.rumia_State = Stage1_1Scene.Rumia.find;
        if (start_ID == 2) {
            //セリフの変更
            Change_Message_Status("RumiaText", 5, 5);
            //宝箱出す
            if (CollectionManager.Instance.Is_Collected("Rumia"))
                return;
            var box = transform.GetChild(0).gameObject;
            box.transform.position = new Vector3(766f, -19f);
            box.transform.SetParent(null);
            box.SetActive(true);
        }
    }    


}
