using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YukaTalk : TalkCharacter {

    protected override float Action_Before_Talk() {
        if(start_ID == 1) {
            BackGroundEffector.Instance.Start_Change_Color(new Color(0.9f, 0.8f, 0.8f), 0.1f);
        }
        else if(start_ID == 2) {
            BackGroundEffector.Instance.Start_Change_Color(new Color(0.7f, 0.6f, 0.6f), 0.1f);
        }
        else {
            BackGroundEffector.Instance.Start_Change_Color(new Color(0.5f, 0.2f, 0.2f), 0.1f);
        }
        return 0;
    }

    protected override void Action_In_End_Talk() {
        if(start_ID == 1) {
            Change_Message_Status("YukaText", 2, 2);            
        }
        else if(start_ID == 2) {
            Change_Message_Status("YukaText", 3, 3);
        }
        else {
            //戦闘開始
            Start_Battle();
        }
    }


    //戦闘開始の処理
    private void Start_Battle() {
        Debug.Log("Start Battle");
        GetComponent<YukaAttack>().Start_Battle();
        mark_Up_Baloon.SetActive(false);
        Destroy(this);
    }
}
