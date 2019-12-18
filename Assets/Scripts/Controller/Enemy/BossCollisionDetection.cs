using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCollisionDetection : EnemyCollisionDetection {    

    private BossEnemy _boss_Enemy;
    

    private void Awake() {
        _boss_Enemy = GetComponent<BossEnemy>();
        Change_Damaged_Tag_Dictionary();
    }


    //被弾の処理
    protected override void Damaged(string key) {
        int damage = (int)(damaged_Tag_Dictionary[key] * Damage_Rate());
        _boss_Enemy.Damaged(damage);
    }


    //被弾タグの設定
    //キックの火力を下げる
    protected override void Change_Damaged_Tag_Dictionary() {
        base.Change_Damaged_Tag_Dictionary();
        damaged_Tag_Dictionary["PlayerKickTag"] = 4;
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
