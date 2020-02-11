using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shizuha : TalkCharacter {

    protected override void Action_In_End_Talk() {
        base.Put_Out_Collection_Box();
    }
}
