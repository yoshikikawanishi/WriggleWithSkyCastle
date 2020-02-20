using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFrogShrine : Enemy {

    //消滅時大蝦蟇戦開始
    public override void Vanish() {
        BigFrogMovie.Instance.Start_Battle_Movie();
    }

}
