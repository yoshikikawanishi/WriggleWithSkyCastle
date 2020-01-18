using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemunoAttack : MonoBehaviour {

    //コンポーネント
    private Rigidbody2D _rigid;
    private NemunoController _controller;
    private MoveTwoPoints _move_Two_Points;

    private float default_Gravity;    

    private bool[] start_Phase = { true, true };


    private void Awake() {
        //取得
        _rigid = GetComponent<Rigidbody2D>();
        _controller = GetComponent<NemunoController>();
        _move_Two_Points = GetComponent<MoveTwoPoints>();

        default_Gravity = _rigid.gravityScale;
    }

    // Use this for initialization
    void Start () {
        StartCoroutine("Back_Jump_Cor", 1);
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


    private IEnumerator Close_Range_Slash() {
        yield return null;
    }

    private IEnumerator Long_Range_Slash() {
        yield return null;
    }


    private void Phase1_Shoot() {

    }

    private void Phase2_Shoot() {

    }

    #endregion
}
