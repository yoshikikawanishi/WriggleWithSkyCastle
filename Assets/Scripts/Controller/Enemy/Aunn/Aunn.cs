using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aunn : BossEnemy {

    //コンポーネント
    private AunnAttack _attack;
    private AunnBGMManager _BGM = new AunnBGMManager();

    //戦闘開始
    private bool start_Battle = false;


    new void Awake() {
        base.Awake();
        //取得
        _attack = GetComponent<AunnAttack>();
    }


    new void Update() {
        base.Update();
        if (start_Battle) {
            switch (Get_Now_Phase()) {
                case 1: _attack.Phase1(_BGM); break;
                case 2: _attack.Phase2(_BGM); break;
            }
        }
    }


    //戦闘開始
    public void Start_Battle() {
        start_Battle = true;
    }

    //クリア時の処理
    public void Clear() {
        start_Battle = false;
        _attack.Stop_Phase2();
    }
}
