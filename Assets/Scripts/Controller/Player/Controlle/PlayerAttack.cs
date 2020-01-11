using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {


    //コンポーネント
    private PlayerController _controller;
    private PlayerSoundEffect player_SE;
    private PlayerEffect player_Effect;
    private Animator _anim;
    private Rigidbody2D _rigid;
    private PlayerAttackCollision attack_Collision;
    private PlayerKickCollision kick_Collision;
    private PlayerManager player_Manager;

    private bool can_Attack = true;    


    // Use this for initialization
    void Awake () {
        //取得
        _controller = GetComponent<PlayerController>();
        player_SE = GetComponentInChildren<PlayerSoundEffect>();
        player_Effect = GetComponentInChildren<PlayerEffect>();
        _anim = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody2D>();
        attack_Collision = GetComponentInChildren<PlayerAttackCollision>();
        kick_Collision = GetComponentInChildren<PlayerKickCollision>();
        player_Manager = PlayerManager.Instance;        
    }


    #region Attack

    //攻撃用フィールド変数
    private float attack_Time = 0.18f;
    private float attack_Span = 0.17f;
    private bool hit_Stop = true;

    private void Set_Attack_Status(float attack_Time, float attack_Span, bool hit_Stop) {
        this.attack_Time = attack_Time;
        this.attack_Span = attack_Span;
        this.hit_Stop = hit_Stop;
    }


    //攻撃
    public void Attack() {
        if (can_Attack) {
            //オプションによって変える
            switch (player_Manager.Get_Option()) {
                case PlayerManager.Option.none:     Set_Attack_Status(0.18f, 0.17f, true); break;
                case PlayerManager.Option.bee:      Set_Attack_Status(0.10f, 0.08f, true); break;
                case PlayerManager.Option.butterfly: Set_Attack_Status(0.18f, 0.17f, false); break;
                case PlayerManager.Option.mantis:   Set_Attack_Status(0.24f, 0.40f, true); break;
                case PlayerManager.Option.spider:   Set_Attack_Status(0.18f, 0.17f, true); break;
            }
            StartCoroutine("Attack_Cor");
        }
    }


    //攻撃用コルーチン
    public IEnumerator Attack_Cor() {
        can_Attack = false;       
        _anim.SetTrigger("AttackTrigger");
        attack_Collision.Make_Collider_Appear(attack_Time);        
        player_SE.Play_Attack_Sound();

        if (player_Manager.Get_Option() == PlayerManager.Option.bee)    //オプションが蜂の時ショット
            Bee_Shoot();

        _rigid.velocity += new Vector2(transform.localScale.x * 5f, 0); //Rigidbodyのスリープ状態を解除する
        for (float t = 0; t < attack_Time; t += Time.deltaTime) {
            //敵と衝突時
            if (attack_Collision.Hit_Trigger()) {
                StartCoroutine("Do_Hit_Attack_Process");
                yield return new WaitForSeconds(0.05f);                
                break;
            }
            yield return null;
        }

        yield return new WaitForSeconds(attack_Span);
        can_Attack = true;
    }

    //オプションが蜂の時のショット    
    private void Bee_Shoot() {
        ObjectPool bullet_Pool = ObjectPoolManager.Instance.Get_Pool("PlayerBeeBullet");
        var bullet = bullet_Pool.GetObject();
        bullet.transform.position = transform.position;
        bullet.transform.localScale = transform.localScale;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(900f * transform.localScale.x, 0);
        player_SE.Play_Shoot_Sound();
    }
    
    //敵と衝突時の処理
    private IEnumerator Do_Hit_Attack_Process() {
        float force = _controller.is_Landing ? 170f : 60f;                  //ノックバック
        _rigid.velocity = new Vector2(force * -transform.localScale.x, 20f);
        BeetlePowerManager.Instance.StartCoroutine("Increase_Cor", 8);      //緑パワーの増加
        player_SE.Play_Hit_Attack_Sound();                                  //効果音        
        if (hit_Stop) {                                                     //ヒットストップ
            float tmp = Time.timeScale;
            Time.timeScale = 0.5f;
            yield return new WaitForSeconds(0.05f);
            Time.timeScale = tmp;
        }
    }

    #endregion 


    #region Kick  

    //キック用フィールド変数
    private bool end_Kick = false;


    //キック
    public void Kick() {
        if (can_Attack) {
            kick_Collision.is_Hit_Kick = false;
            if (_controller.is_Landing) 
                StartCoroutine("Kick_Cor", true);            
            else 
                StartCoroutine("Kick_Cor", false);
        }        
    }


    private IEnumerator Kick_Cor(bool is_Sliding) {
        can_Attack = false;
        _controller.Set_Is_Playable(false);
        _controller.Change_Animation("KickBool");

        //キック開始
        _rigid.velocity = new Vector2(transform.localScale.x * 180f, -200f);
        kick_Collision.Make_Collider_Appear();
        player_SE.Play_Kick_Sound();

        //キック中
        if (is_Sliding) 
            StartCoroutine("Sliding_Cor");        
        else 
            StartCoroutine("Kicking_Cor");

        yield return new WaitUntil(End_Kick);    

        //キック終了
        if (_controller.is_Landing)
            _controller.Change_Animation("IdleBool");
        else
            _controller.Change_Animation("JumpBool");        

        _controller.Set_Is_Playable(true);
        kick_Collision.Make_Collider_Disappear();

        float time = is_Sliding ? 0 : 0.05f;
        yield return new WaitForSeconds(time);
        
        can_Attack = true;
    }


    //キック発生中の処理
    private IEnumerator Kicking_Cor() {
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
                if (t < 0.2f)
                    t = 0.2f;
            }

            yield return null;
        }
        end_Kick = true;
    }

    //スライディング発生中の処理
    private IEnumerator Sliding_Cor() {
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
        end_Kick = true;
    }


    //キックのヒット時の処理
    private void Do_Hit_Kick_Process() {
        _rigid.velocity = new Vector2(40f * -transform.localScale.x, 200f); //ノックバック
        BeetlePowerManager.Instance.StartCoroutine("Increase_Cor", 8);      //緑ゲージの増加
        player_SE.Play_Hit_Attack_Sound();                                  //効果音
    }


    //キック終了の確認用
    private bool End_Kick() {
        if (end_Kick) {
            end_Kick = false;
            return true;
        }
        return false;
    }
    #endregion
}
