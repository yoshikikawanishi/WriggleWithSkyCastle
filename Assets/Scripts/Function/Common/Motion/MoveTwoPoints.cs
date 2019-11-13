using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTwoPoints : MonoBehaviour {

    private bool end_Move = false;
    private bool is_Local_Position = false;

    private float arc_Size  = 0;
    private float max_Speed = 0.02f;
    private float acc_Rate  = 1.2f;
    private float dec_Rate  = 0.9f;


    //移動開始
    public void Start_Move(Vector3 next_Pos, float arc_Size, bool is_Local_Position) {
        end_Move = false;
        this.arc_Size = arc_Size;
        this.is_Local_Position = is_Local_Position;
        StopCoroutine("Move_Two_Points");
        StartCoroutine("Move_Two_Points", next_Pos);
    }


    //速度の設定
    public void Set_Speed(float max_Speed, float acc_Rate, float dec_Rate) {
        if(max_Speed > 0) {
            this.max_Speed = max_Speed;
        }
        if(acc_Rate > 1.0f) {
            this.acc_Rate = acc_Rate;
        }
        if(0 < dec_Rate && dec_Rate < 1.0f) {
            this.dec_Rate = dec_Rate;
        }
    }


    //移動用のコルーチン
    private IEnumerator Move_Two_Points(Vector3 next_Pos) {
        float speed         = 0.001f;    //速度
        float now_Location  = 0;    //現在の移動距離割合
        Vector3 start_Pos = transform.position;
        Vector3 pos = start_Pos;

        while (now_Location <= 1) {
            now_Location += speed * Time.timeScale;
            pos = Vector3.Lerp(start_Pos, next_Pos, now_Location);  //直線の軌道
            pos += new Vector3(0, arc_Size * Mathf.Sin(now_Location * Mathf.PI), 0); //弧の軌道
            if (!is_Local_Position) {
                transform.position = pos; 
            }
            else {
                transform.localPosition = pos;
            }
            //加速
            if (speed <= max_Speed && Time.timeScale != 0) {
                speed *= acc_Rate;
            }
            //減速
            if (now_Location >= 0.8f && Time.timeScale != 0) {
                speed *= dec_Rate;
            }
            yield return new WaitForSeconds(0.016f);
        }

        end_Move = true;
    }


    //移動終了検知用
    public bool End_Move() {
        if (end_Move) {
            end_Move = false;
            return true;
        }
        return false;
    }
}
