using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aya : TalkCharacter {

    new void Start() {
        base.Start();
        CollectionManager c = CollectionManager.Instance;
        if (c.Is_Collected("Momizi") && c.Is_Collected("Aya")) {
            Change_Status_With_Momizi();
        }
    }

    protected override void Action_In_End_Talk() {
        if(start_ID == 1) {
            Put_Out_Collection_Box();
            start_ID = 24;
            end_ID = 24;
        }
    }


    private void Change_Status_With_Momizi() {
        Change_Message_Status("AyaText", 25, 25);
    }
}
