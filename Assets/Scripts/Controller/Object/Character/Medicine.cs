using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medicine : TalkCharacter {

    protected override void Action_In_End_Talk() {
        Put_Out_Collection_Box();
    }
}
