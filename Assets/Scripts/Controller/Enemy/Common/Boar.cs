using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy {

    private GameObject player;
    private Rigidbody2D _rigid;

    private bool is_Rushing = false;
    private float walk_Time = 0;


	// Use this for initialization
	void Start () {
        //取得
        player = GameObject.FindWithTag("PlayerTag");
        _rigid = GetComponent<Rigidbody2D>();
	}

	
	// Update is called once per frame
	void Update () {
        //自機を探す
        if (Search_Player()) {
            //突進開始
            if (!is_Rushing) {
                StartCoroutine("Start_Rush_Cor");
            }
            //突進
            else if (Mathf.Abs(_rigid.velocity.x) < 180f) {
                _rigid.velocity += new Vector2(-transform.localScale.x * 5, 0);
            }
        }
        else {
            is_Rushing = false;
            //自機を見失って減速
            if (Mathf.Abs(_rigid.velocity.x) >= 0.01f) {
                _rigid.velocity *= new Vector2(0.995f, 1);
            }
            //自機未発見時歩く
            else {
                if(walk_Time < 2.0f) {
                    walk_Time += Time.deltaTime;
                    transform.position += new Vector3(transform.localScale.x * -0.2f, 0);
                }
                else {
                    transform.localScale = transform.localScale * new Vector2(-1, 1);
                    walk_Time = 0;
                }
            }
        }

    }


    //正面にいる自機を見つける
    private bool Search_Player() {
        Vector2 distance = player.transform.position - transform.position;
        distance *= new Vector2(transform.localScale.x, 1);
        if(-256f < distance.x && distance.x < 0) {
            if(Mathf.Abs(distance.y) < 96f) {
                return true;
            }
        }
        return false;
    }


    //突進開始
    private IEnumerator Start_Rush_Cor() {
        yield return new WaitForSeconds(0.5f);
        is_Rushing = true;
    }


    //攻撃を受けると後ずさる
    //direction == 1 で右方向に後ずさる -1で左
    public void Guard(int direction) {
        if (is_Rushing) {
            GetComponent<Rigidbody2D>().velocity = new Vector2(direction * 80f, 5f);
        }
        transform.localScale = new Vector3(direction, 1);        
    }

}
