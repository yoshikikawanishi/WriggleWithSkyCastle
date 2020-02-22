using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nitori : TalkCharacter {

    protected override float Action_Before_Talk() {        
        //点滅して登場
        if(start_ID == 2) {
            StartCoroutine("Blink_Cor");
            return 0.6f;
        }
        return 0;
    }

    protected override void Action_In_End_Talk() {
        //驚きセリフ後、登場セリフ
        if (start_ID == 1) {            
            Change_Message_Status("NitoriText", 2, 2);
            StartCoroutine(Talk());
        }
        //イベント終了後
        else if(start_ID == 2) {
            Put_Out_Collection_Box();
            Change_Message_Status("NitoriText", 3, 3);
        }
    }

    //点滅して登場
    private IEnumerator Blink_Cor() {
        SpriteRenderer _sprite = GetComponent<SpriteRenderer>();
        for(int i = 0; i < 5; i++) {
            _sprite.color = new Color(1, 1, 1, 0.4f);
            yield return new WaitForSeconds(0.05f);
            _sprite.color = new Color(1, 1, 1, 1f);
            yield return new WaitForSeconds(0.05f);
        }
        _sprite.sortingOrder = 5;
    }
}
