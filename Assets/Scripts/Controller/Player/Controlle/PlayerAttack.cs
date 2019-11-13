using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    //コンポーネント
    private PlayerController _controller;
    private Animator _anim;
    private Rigidbody2D _rigid;
    private PlayerAttackCollision attack_Collision;
    private PlayerKickCollision kick_Collision;

    private bool can_Attack = true;


	// Use this for initialization
	void Awake () {
        //取得
        _controller = GetComponent<PlayerController>();
        _anim = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody2D>();
        attack_Collision = GetComponentInChildren<PlayerAttackCollision>();
        kick_Collision = GetComponentInChildren<PlayerKickCollision>();
    }


    //攻撃
    public void Attack() {
        if (can_Attack) {
            StartCoroutine("Attack_Cor");
        }
    }


    public IEnumerator Attack_Cor() {
        can_Attack = false;       
        _anim.SetTrigger("AttackTrigger");

        attack_Collision.Make_Collider_Appear(0.18f);
        _rigid.velocity += new Vector2(transform.localScale.x * 5f, 0); //Rigidbodyのスリープ状態を解除する
        for (float t = 0; t < 0.18f; t += Time.deltaTime) {
            //敵と衝突時ノックバック
            if (attack_Collision.Hit_Trigger()) {
                Occur_Knock_Back();
                BeetlePowerManager.Instance.StartCoroutine("Increase_Cor", 8);
                //ヒットストップ
                Time.timeScale = 0.5f;
                yield return new WaitForSeconds(0.05f);
                Time.timeScale = 1.0f;
                break;
            }
            yield return null;
        }

        yield return new WaitForSeconds(0.17f);
        can_Attack = true;
    }

    //ノックバック
    private void Occur_Knock_Back() {
        float force = _controller.is_Landing ? 170f : 40f;
        _rigid.velocity = new Vector2(force * -transform.localScale.x, 20f);
    }


    //キック
    public void Kick() {
        if (can_Attack) {
            StartCoroutine("Kick_Cor");
        }        
    }


    private IEnumerator Kick_Cor() {
        can_Attack = false;
        _controller.Set_Is_Playable(false);
        _anim.SetTrigger("KickTrigger");
        _rigid.velocity = new Vector2(transform.localScale.x * 180f, -200f);

        kick_Collision.Make_Collider_Appear();
        for (float t = 0; t < 0.33f; t += Time.deltaTime) {
            _rigid.velocity = new Vector2(transform.localScale.x * 180f, _rigid.velocity.y);
            //敵と衝突時ノックバック
            if (kick_Collision.Hit_Trigger()) {                
                _rigid.velocity = new Vector2(40f * -transform.localScale.x, 180f);
                BeetlePowerManager.Instance.StartCoroutine("Increase_Cor", 8);
                _controller.Change_Animation("JumpBool");
                yield return new WaitForSeconds(0.15f);
                break;
            }
            yield return null;
        }
        kick_Collision.Make_Collider_Disappear();

        _controller.Set_Is_Playable(true);
        can_Attack = true;
    }
}
