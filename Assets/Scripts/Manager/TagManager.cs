using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagManager  {

    //着地判定
    public static readonly List<string> LAND_TAG_LIST = new List<string>()
    {
        "GroundTag", "ThroughGroundTag", "DamageGroundTag"
    };


    //自機の被弾判定
    public static readonly List<string> PLAYER_DAMAGED_TAG_LIST = new List<string>()
    {
        "EnemyTag", "EnemyBulletTag", "DamageGroundTag"
    };

}
