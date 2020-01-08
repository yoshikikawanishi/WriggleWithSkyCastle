using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YukaAttack : MonoBehaviour {

    //コンポーネント
    private BossEnemy _boss_Controller;
    private MoveTwoPoints _move_Two_Points;


    // Use this for initialization
    void Start() {
        //取得
        _boss_Controller = GetComponent<BossEnemy>();
        _move_Two_Points = GetComponent<MoveTwoPoints>();
        //ボス戦開始前無敵化
        _boss_Controller.Set_Is_Invincible(true);
    }

    // Update is called once per frame
    void Update() {
        //クリア
        if (_boss_Controller.Clear_Trigger()) {
            Clear();
        }
    }


    //戦闘開始
    public void Start_Battle() {        
        //ボス敵にする
        _boss_Controller.Set_Is_Invincible(false);
        gameObject.tag = "EnemyTag";
        //攻撃開始
        StartCoroutine("Attack_Cor");
        //カメラの固定
        GameObject camera = GameObject.FindWithTag("MainCamera");
        camera.AddComponent<MoveTwoPoints>().Start_Move(new Vector3(transform.position.x, 0, -10f));
        camera.GetComponent<CameraController>().enabled = false;
    }
    

    //攻撃
    private IEnumerator Attack_Cor() {
        _move_Two_Points.Start_Move(new Vector3(transform.position.x, 0));
        yield return new WaitUntil(_move_Two_Points.End_Move);            
    }


    //クリア時の処理
    public void Clear() {
        StopAllCoroutines();
        GameObject camera = GameObject.FindWithTag("MainCamera");        
        camera.GetComponent<CameraController>().enabled = true;
        BackGroundEffector.Instance.Start_Change_Color(new Color(1, 1, 1), 0.1f);
    }
}
