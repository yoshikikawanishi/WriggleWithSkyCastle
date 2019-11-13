using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAccelerator : MonoBehaviour {


    /// <summary>
    /// 弾をまっすぐ加速させる
    /// </summary>
    /// <param name="bullet_List"></param>
	public void Accelerat_Bullet(List<GameObject> bullet_List, float acc_Rate) {
        if (bullet_List[0].GetComponent<Rigidbody2D>() == null) {
            Debug.Log("Bullet Not Attached Rigidbody");
            return;
        }
        StartCoroutine("Accelerate_Bullet_Routine");          
    }


    //加速させるコルーチン
    private IEnumerator Accelerate_Bullet_Routine(List<GameObject> bullet_List, float acc_Rate, float acc_Time) {
        //Rigidbody2Dの取得
        List<Rigidbody2D> rigid_List = new List<Rigidbody2D>();
        for(int i = 0; i < bullet_List.Count; i++) {
            rigid_List.Add(bullet_List[i].GetComponent<Rigidbody2D>());
        }
        //加速
        for (float t = 0; t < acc_Time; t += Time.deltaTime) {
            for(int i = 0; i < bullet_List.Count; i++) {
                rigid_List[i].velocity *= acc_Rate * Time.timeScale;
            }
            yield return null;
        }
    }
}
