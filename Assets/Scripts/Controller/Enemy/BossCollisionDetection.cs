using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCollisionDetection : EnemyCollisionDetection {    

    private BossEnemy _boss_Enemy;
    

    private void Awake() {
        _boss_Enemy = GetComponent<BossEnemy>();

        //キックの火力を下げる
        damaged_Tag_Dictionary["PlayerKickTag"] = 6;
    }


    //被弾の処理
    protected override void Damaged(string key) {
        int damage = (int)(damaged_Tag_Dictionary[key] * Damage_Rate());
        _boss_Enemy.Damaged(damage, key);
    }


    //無敵化
    public void Become_Invincible() {
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
    }

    //無敵化解除
    public void Release_Invincible() {
        gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
    }

}
