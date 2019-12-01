using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    //コンポーネント
    private PlayerController _controller;
    private PlayerSoundEffect player_SE;
    private Animator _anim;
    private Rigidbody2D _rigid;
    private PlayerAttackCollision attack_Collision;
    private PlayerKickCollision kick_Collision;

    private bool can_Attack = true;


	// Use this for initialization
	void Awake () {
        //取得
        _controller = GetComponent<PlayerController>();
        player_SE = GetComponentInChildren<PlayerSoundEffect>();
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
        player_SE.Play_Attack_Sound();
        _rigid.velocity += new Vector2(transform.localScale.x * 5f, 0); //Rigidbodyのスリープ状態を解除する
        for (float t = 0; t < 0.18f; t += Time.deltaTime) {
            //敵と衝突時ノックバック
            if (attack_Collision.Hit_Trigger()) {
                Occur_Knock_Back();
                BeetlePowerManager.Instance.StartCoroutine("Increase_Cor", 8);
                player_SE.Play_Hit_Attack_Sound();
                //ヒットストップ
                float tmp = Time.timeScale;
                Time.timeScale = 0.5f;
                yield return new WaitForSeconds(0.05f);
                Time.timeScale = tmp;
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
            kick_Collision.is_Hit_Kick = false;
            if (_controller.is_Landing) 
                StartCoroutine("Sliding_Cor");            
            else 
                StartCoroutine("Kick_Cor");
        }        
    }


    private IEnumerator Kick_Cor() {
        can_Attack = false;
        _controller.Set_Is_Playable(false);
        _controller.Change_Animation("KickBool");

        //キック開始
        _rigid.velocity = new Vector2(transform.localScale.x * 180f, -200f);
        kick_Collision.Make_Collider_Appear();
        player_SE.Play_Kick_Sound();

        for (float t = 0; t < 0.3f; t += Time.deltaTime) {

            _rigid.velocity = new Vector2(transform.localScale.x * 180f, _rigid.velocity.y);

            //敵と衝突時の処理
            if (kick_Collision.Hit_Trigger()) {
                Do_Hit_Kick_Process();
                yield return new WaitForSeconds(0.015f);
                break;
            }

            //着地時終了
            if (_controller.is_Landing) {
                _rigid.velocity = new Vector2(transform.localScale.x * 180f, _rigid.velocity.y);
                _controller.Change_Animation("KickBool");
                if(t < 0.2f)
                    t = 0.2f;
            }

            yield return null;
        }

        if (_controller.is_Landing)
            _controller.Change_Animation("IdleBool");
        else
            _controller.Change_Animation("JumpBool");        

        _controller.Set_Is_Playable(true);
        kick_Collision.Make_Collider_Disappear();

        yield return new WaitForSeconds(0.05f);
        
        can_Attack = true;
    }


    //スライディング
    private IEnumerator Sliding_Cor() {
        can_Attack = false;
        _controller.Set_Is_Playable(false);
        _controller.Change_Animation("KickBool");

        _rigid.velocity = new Vector2(transform.localScale.x * 180f, -200f);
        kick_Collision.Make_Collider_Appear();
        player_SE.Play_Kick_Sound();

        //スライディング開始
        for (float t = 0; t < 0.33f; t += Time.deltaTime) {

            _rigid.velocity = new Vector2(transform.localScale.x * 180f, _rigid.velocity.y);
            _controller.Change_Animation("KickBool");

            //敵と衝突時の処理
            if (kick_Collision.Hit_Trigger()) {
                Do_Hit_Kick_Process();
                
                yield return new WaitForSeconds(0.015f);
                break;
            }            

            yield return null;
        }

        if (_controller.is_Landing)
            _controller.Change_Animation("IdleBool");
        else
            _controller.Change_Animation("JumpBool");        

        _controller.Set_Is_Playable(true);
        kick_Collision.Make_Collider_Disappear();        

        can_Attack = true;
    }


    //キックのヒット時の処理
    private void Do_Hit_Kick_Process() {
        _rigid.velocity = new Vector2(40f * -transform.localScale.x, 200f); //ノックバック
        BeetlePowerManager.Instance.StartCoroutine("Increase_Cor", 8);      //緑ゲージの増加
        player_SE.Play_Hit_Attack_Sound();                                  //効果音
    }
}
