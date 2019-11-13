using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LarvaController : MonoBehaviour {

    //コンポーネント
    private LarvaAttack _attack;
    private BossEnemy _boss;


    private void Awake() {
        //取得
        _attack = GetComponent<LarvaAttack>();
        _boss = GetComponent<BossEnemy>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        switch (_boss.Get_Now_Phase()) {
            case 1: _attack.Do_Phase1(); break;
            case 2: _attack.Do_Phase2(); break;
        }
	}


    //クリア
    public void Clear() {
        _attack.Stop_Phase2();
    }
}
