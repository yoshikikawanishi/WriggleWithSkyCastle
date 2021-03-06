﻿using System.Collections;
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
        if(start_ID == 41) {
            Put_Out_Collection_Box();
            start_ID = 51;
            end_ID = 52;
        }
    }


    private void Change_Status_With_Momizi() {
        Change_Message_Status("AyaText", 53, 53);
    }
}
