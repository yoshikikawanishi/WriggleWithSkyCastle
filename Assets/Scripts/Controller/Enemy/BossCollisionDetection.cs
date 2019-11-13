using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCollisionDetection : EnemyCollisionDetection {    

    private BossEnemy _boss_Enemy;


    private void Awake() {
        _boss_Enemy = GetComponent<BossEnemy>();    
    }


    //被弾の処理
    protected override void Damaged(string key) {
        int damage = (int)(damaged_Tag_Dictionary[key] * Damage_Rate());
        _boss_Enemy.Damaged(damage);
    }

}
