using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemunoController : MonoBehaviour {

    //コンポーネント
    private Animator _anim;
    private BossEnemy _boss_Controller;
    private NemunoAttack _attack;

    //戦闘開始
    private bool start_Battle = false;


    private void Awake() {
        //取得
        _anim = GetComponent<Animator>();
        _boss_Controller = GetComponent<BossEnemy>();
        _attack = GetComponent<NemunoAttack>();
    }


    // Use this for initialization
    void Start () {
	}
	

	// Update is called once per frame
	void Update () {
        if (start_Battle) {
            switch (_boss_Controller.Get_Now_Phase()) {
                case 1: _attack.Phase1(); break;
                case 2: _attack.Phase2(); break;
            }
        }
	}


    //アニメーション変更
    public void Change_Animation(string next) {
        foreach(AnimatorControllerParameter param in _anim.parameters) {
            _anim.SetBool(param.name, false);
        }
        _anim.SetBool(next, true);
    }


    //戦闘開始
    public void Start_Battle() {
        start_Battle = true;
    }
}
