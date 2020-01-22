using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemunoController : MonoBehaviour {

    //コンポーネント
    private Animator _anim;
    private Rigidbody2D _rigid;
    private CapsuleCollider2D _collider;
    private BossEnemy _boss_Controller;
    private NemunoAttack _attack;

    //戦闘開始
    private bool start_Battle = false;


    private void Awake() {
        //取得
        _anim = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
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
            if(param.name.Contains("Bool"))
                _anim.SetBool(param.name, false);            
        }
        if(next.Contains("Bool"))
            _anim.SetBool(next, true);
        if (next.Contains("Trigger"))
            _anim.SetTrigger(next);
    }


    //戦闘開始
    public void Start_Battle() {
        start_Battle = true;
    }


    //空に飛ぶとき、重力を消してあたりはんていをtriggerにする
    public void Change_Fly_Parameter() {
        _rigid.gravityScale = 0;
        _collider.isTrigger = true;
    }

    //地上に降りるとき、重力を付けて当たり判定をつける
    public void Change_Land_Paramter() {
        _rigid.gravityScale = 32f;
        _collider.isTrigger = false;
    }


    //-------------エフェクト--------------
    public void Play_Charge_Effect(float duration) {
        transform.Find("Effects").GetChild(0).gameObject.SetActive(true);
        Invoke("Stop_Charge_Effect", duration);
    }

    public void Stop_Charge_Effect() {
        transform.Find("Effects").GetChild(0).gameObject.SetActive(false);
    }

    public void Play_Small_Charge_Effect() {
        transform.Find("Effects").GetChild(1).GetComponent<ParticleSystem>().Play();
    }

    public void Play_Burst_Effect() {
        transform.Find("Effects").GetChild(2).GetComponent<ParticleSystem>().Play();
    }

    public void Play_Small_Burst_Effect() {
        transform.Find("Effects").GetChild(3).GetComponent<ParticleSystem>().Play();
    }

}
