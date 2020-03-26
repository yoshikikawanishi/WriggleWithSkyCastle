using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AunnAttack : MonoBehaviour {

    //コンポーネント
    private AunnAttackFunction _attack_Func;    


    private void Awake() {
        //取得
        _attack_Func = GetComponent<AunnAttackFunction>();
    }    
	
	
    //フェーズ１
    public void Phase1(AunnBGMManager _BGM) {

    }

    public void Stop_Phase1() {

    }


    //フェーズ２
    public void Phase2(AunnBGMManager _BGM) {

    }

    public void Stop_Phase2() {

    }
}
