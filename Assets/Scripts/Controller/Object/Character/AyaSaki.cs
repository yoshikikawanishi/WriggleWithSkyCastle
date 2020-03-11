using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AyaSaki : TalkCharacter {

    protected override void Action_In_End_Talk() {
        if(talk_Count == 1) {
            Put_Out_Collection_Box();
            start_ID = 2;
            end_ID = 2;
        }
    }
}
