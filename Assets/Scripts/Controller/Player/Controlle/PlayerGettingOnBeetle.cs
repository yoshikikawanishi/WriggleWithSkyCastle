using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGettingOnBeetle : MonoBehaviour {

    [SerializeField] private GameObject beetle_Prefab;
    private GameObject beetle;

    private GameObject main_Camera;
    private GameObject beetle_Body;

    private PlayerController _controller;
    private PlayerBodyCollision body_Collision;
    private PlayerEffect player_Effect;

    public bool can_Get_On_Beetle = true;

    private float default_Gravity;
    private Vector2 default_Collider_Offset;


    private float scroll_Speed = 1f;


    //Start
    private void Awake() {
        //取得
        main_Camera = GameObject.FindWithTag("MainCamera");
        beetle_Body = transform.Find("BeetleBody").gameObject;
        body_Collision = GetComponentInChildren<PlayerBodyCollision>();
        _controller = GetComponent<PlayerController>();
        player_Effect = GetComponentInChildren<PlayerEffect>();

        //初期値代入
        default_Gravity = GetComponent<Rigidbody2D>().gravityScale;
        default_Collider_Offset = GetComponentInChildren<CapsuleCollider2D>().offset;
    }


    #region Get On Beetle

    //カブトムシに乗る
    public void Get_On_Beetle() {
        if (!can_Get_On_Beetle) {
            return;
        }
        _controller.is_Ride_Beetle = true;
        StopAllCoroutines();
        StartCoroutine("Get_On_Beetle_Cor");
    }

    private IEnumerator Get_On_Beetle_Cor() {
        can_Get_On_Beetle = false;

        player_Effect.Start_Ridding_Beetle_Effect();
        _controller.Change_Animation("RideBeetleBool");
        Change_To_Beetle_Status();

        yield return new WaitForSeconds(0.4f);
        player_Effect.Stop_Ridding_Beetle_Effect();        
        
        can_Get_On_Beetle = true;
    }


    //ステータス変更
    private void Change_To_Beetle_Status() {
        transform.SetParent(main_Camera.transform);                                         //カメラの子に        
        GetComponent<Rigidbody2D>().gravityScale = 0;                                       //重力
        body_Collision.Change_Collider_Size(new Vector2(6f, 6f), default_Collider_Offset);  //当たり判定
        body_Collision.Display_Sprite();
        beetle_Body.SetActive(true);
        main_Camera.GetComponent<CameraController>().Start_Auto_Scroll(scroll_Speed);  //オートスクロール        
    }

    #endregion


    #region Get Off Beetle

    //カブトムシから降りる
    public void Get_Off_Beetle() {
        if (!can_Get_On_Beetle) {
            return;
        }
        StopAllCoroutines();
        StartCoroutine("Get_Off_Beetle_Cor");                
    }    

    private IEnumerator Get_Off_Beetle_Cor() {
        can_Get_On_Beetle = false;
       
        //ステータス変更
        Change_To_Default_Status();
        //カブトムシ退場
        StartCoroutine("Leaving_Beetle_Cor");        
        _controller.is_Ride_Beetle = false;

        yield return new WaitForSeconds(0.4f);
        can_Get_On_Beetle = true;
    }

    //カブトムシ退場
    public IEnumerator Leaving_Beetle_Cor() {
        //向き
        int direction = _controller.Get_Beetle_Direction();
        //生成
        if (beetle == null)
            beetle = Instantiate(beetle_Prefab);
        else
            beetle.SetActive(true);
        beetle.transform.position = gameObject.transform.position;
        beetle.transform.localScale = new Vector3(direction, 1, 1);
        //移動 
        float speed = 0.035f;    //速度
        float now_Location = 0;  //現在の移動距離割合
        Vector3 start_Pos = gameObject.transform.position;
        Vector3 next_Pos = main_Camera.transform.position + new Vector3(280f * direction, 160f);
        Vector3 pos = start_Pos;
        while (now_Location <= 1) {
            now_Location += speed;
            pos = Vector3.Lerp(start_Pos, next_Pos, now_Location);  //直線の軌道
            pos += new Vector3(0, -48 * Mathf.Sin(now_Location * Mathf.PI), 0); //弧の軌道
            beetle.transform.position = pos;
            yield return null;
        }
        beetle.SetActive(false);
    }

    //ステータス変更
    private void Change_To_Default_Status() {        
        string anim_Parm = _controller.is_Landing ? "IdleBool" : "JumpBool";    //アニメーション
        _controller.Change_Animation(anim_Parm);

        transform.SetParent(null);                                      //親子関係解除                 
        GetComponent<Rigidbody2D>().gravityScale = default_Gravity;     //重力
        body_Collision.Back_Default_Collider();                         //当たり判定
        body_Collision.Hide_Sprite();
        beetle_Body.SetActive(false);
        main_Camera.GetComponent<CameraController>().Quit_Auto_Scroll();//オートスクロール
    }

    #endregion


    //飛行無効化
    public void To_Disable() {
        if (_controller.is_Ride_Beetle) {
            Get_Off_Beetle();
        }
        can_Get_On_Beetle = false;
    }

    //無効化解除
    public void To_Enable() {
        can_Get_On_Beetle = true;
    }

    //スクロール速度変更
    public void Set_Scroll_Speed(float speed) {
        scroll_Speed = speed;
        if (_controller.Get_Is_Ride_Beetle()) {
            main_Camera.GetComponent<CameraController>().Start_Auto_Scroll(speed);
        }
    }

    
}
