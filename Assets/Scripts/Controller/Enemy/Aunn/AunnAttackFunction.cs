using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻撃の関数置き場
/// </summary>
public class AunnAttackFunction : MonoBehaviour {

    private Aunn _controller;
    private AunnAttack _attack;
    private AunnShoot _shoot;
    private Rigidbody2D _rigid;
    private MoveMotion _move;

    private ChildColliderTrigger foot_Collision;


    void Awake() {
        //取得
        _controller = GetComponent<Aunn>();
        _attack = GetComponent<AunnAttack>();
        _shoot = GetComponentInChildren<AunnShoot>();
        _rigid = GetComponent<Rigidbody2D>();
        _move = GetComponent<MoveMotion>();
        foot_Collision = transform.Find("Foot").GetComponent<ChildColliderTrigger>();
    }

    void Start() {
            
    }



    //--------------------------攻撃用関数------------------------------
    // ※※※※  攻撃終了時に _attack.can_Attack をtrueにすること  ※※※※


    //潜行、地面から自機追従、ジャンプ、ショット
    public void Dive_And_Jump_Shoot() {
        StartCoroutine("Dive_And_Jump_Shoot_Cor");
    }


    private IEnumerator Dive_And_Jump_Shoot_Cor() {
        //取得
        BossCollisionDetection _collision = GetComponent<BossCollisionDetection>();
        TracePlayer _trace = GetComponent<TracePlayer>();

        //地面に潜る
        _controller.Change_Fly_Parameter();
        _move.Start_Move(0);
        yield return new WaitUntil(_move.Is_End_Move);

        //当たり判定を消して自機を追いかける
        _collision.Become_Invincible();
        _trace.enabled = true;
        yield return new WaitForSeconds(2.2f);
        _trace.enabled = false;

        yield return new WaitForSeconds(0.8f);

        //ジャンプして弾幕発射
        _collision.Release_Invincible();
        _move.Start_Move(1);
        yield return new WaitUntil(_move.Is_End_Move);
        _shoot.Shoot_Short_Curve_Laser();
        yield return new WaitForSeconds(0.5f);

        //落下
        _controller.Change_Land_Parameter();
        yield return new WaitUntil(foot_Collision.Hit_Trigger);

        yield return new WaitForSeconds(0.8f);

        _attack.can_Attack = true;
    }

}
