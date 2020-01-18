using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemunoAttack : MonoBehaviour {

    //コンポーネント
    private Rigidbody2D _rigid;
    private SpriteRenderer _sprite;
    private NemunoController _controller;
    private NemunoShoot _shoot;
    private NemunoSoundEffect _sound;
    private MoveTwoPoints _move_Two_Points;

    private float default_Gravity;    

    private bool[] start_Phase = { true, true };


    private void Awake() {
        //取得
        _rigid = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _controller = GetComponent<NemunoController>();
        _shoot = GetComponentInChildren<NemunoShoot>();
        _sound = GetComponentInChildren<NemunoSoundEffect>();
        _move_Two_Points = GetComponent<MoveTwoPoints>();

        default_Gravity = _rigid.gravityScale;
    }

    // Use this for initialization
    void Start () {
        StartCoroutine(Long_Slash_Cor());
	}


    #region Phase1
    //フェーズ１
    public void Phase1() {
        if (start_Phase[0]) {
            start_Phase[0] = false;
            StartCoroutine("Phase1_Cor");
        }
    }

    private IEnumerator Phase1_Cor() {
        yield return null;
    }
    
    private void Stop_Phase1() {
        StopCoroutine("Phase1_Cor");
    }
    #endregion


    #region Phase2
    //フェーズ２
    public void Phase2() {
        if (start_Phase[1]) {
            start_Phase[1] = false;
            StartCoroutine("Phase2_Cor");
        }
    }

    private IEnumerator Phase2_Cor() {
        yield return null;
    }

    public void Stop_Phase2() {
        StopCoroutine("Phase2_Cor");
    }
    #endregion


    #region AttackFunctions

    //前方向にジャンプする
    private IEnumerator Forward_Jump_Cor(float jump_Distance) {        
        _controller.Change_Animation("ForwardJumpBool");
        yield return new WaitForSeconds(0.2f);
        _rigid.gravityScale = 0;

        _move_Two_Points.Start_Move(new Vector3(jump_Distance, 0), 0);
        yield return new WaitUntil(_move_Two_Points.End_Move);

        _controller.Change_Animation("IdleBool");
        _rigid.gravityScale = default_Gravity;
    }

    //橋の決まった座標にバックジャンプする
    // direction == 1 で右端、direction == -1で左端
    private IEnumerator Back_Jump_Cor(int direction) {
        direction = direction.CompareTo(0);
        transform.localScale = new Vector3(direction, 1, 1);

        _controller.Change_Animation("BackJumpBool");
        yield return new WaitForSeconds(0.2f);
        _rigid.gravityScale = 0;

        _move_Two_Points.Start_Move(new Vector3(160f * direction, transform.position.y), 0);
        yield return new WaitUntil(_move_Two_Points.End_Move);

        _controller.Change_Animation("IdleBool");
        _rigid.gravityScale = default_Gravity;
    }


    //近接攻撃、一回点滅後攻撃
    private IEnumerator Close_Slash_Cor() {        
        _controller.Change_Animation("SlashBool");

        _sound.Play_Before_Slash_Sound();
        yield return new WaitForSeconds(0.13f);
        _sprite.color = new Color(0.7f, 0.7f, 0.7f);        
        yield return new WaitForSeconds(0.13f);
        _sprite.color = new Color(0.5f, 0.5f, 0.5f);
        yield return new WaitForSeconds(0.25f);

        _sound.Play_Slash_Sound();
        yield return new WaitForSeconds(0.5f);

        _controller.Change_Animation("IdleBool");
    }

    //遠距離攻撃、２回点滅後攻撃、ショット
    private IEnumerator Long_Slash_Cor() {
        yield return new WaitForSeconds(2.0f);
        _controller.Change_Animation("SlashBool");

        for(int i = 0; i < 2; i++) {
            _sound.Play_Before_Slash_Sound();
            yield return new WaitForSeconds(0.1f);
            _sprite.color = new Color(0.7f, 0.7f, 0.7f);            
            yield return new WaitForSeconds(0.1f);
            _sprite.color = new Color(0.5f, 0.5f, 0.5f);
        }
        yield return new WaitForSeconds(0.1f);

        _sound.Play_Slash_Sound();
        _shoot.shoot_Shotgun();
        yield return new WaitForSeconds(0.5f);

        _controller.Change_Animation("IdleBool");
    }


    private void Phase1_Shoot() {

    }

    private void Phase2_Shoot() {

    }

    #endregion
}
