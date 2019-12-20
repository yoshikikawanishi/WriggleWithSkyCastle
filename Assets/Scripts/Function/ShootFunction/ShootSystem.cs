using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSystem : MonoBehaviour {
    
    //種類
    public enum KIND {
        Odd,
        Even,
        Diffusion,
        nWay,
        Scatter,
        Spiral
    };
    public KIND kind;

    //基本設定
    public bool play_On_Awake = false;
    public GameObject bullet = null;
    public Transform parent = null;
    public float max_Speed;
    public float lifeTime = 5;

    public bool is_Acceleration = false;
    public AnimationCurve velocity_Forward;
    public AnimationCurve velocity_Lateral;


    //その他設定
    public bool other_Param = true;    
    // 1:奇数段 / 2:偶数弾 / 3:全方位弾 / 4:nWay弾 / 5:ばらまき弾 / 6:渦巻き弾
    public int num = 1;                 //1, 2, 3, 4
    public int count = 5;               //1, 2, 3, 4
    public float span = 1.0f;           //1, 2, 3, 4
    public float inter_Angle_Deg = 20;  //1, 2, 3, 4, 6
    public float center_Angle_Deg = 0;  //2, 3, 4, 5    
    public float arc_Deg = 360f;        //5
    public float shoot_Rate = 30f;      //5, 6  
    public float duration = 5.0f;       //5, 6

    //繰り返し時、値の変更用
    public bool is_Count_Up_Param;
    public int num_Up = 0;
    public float speed_Up = 0;
    public float inter_Angle_Up = 0;
    public float center_Angle_Up = 0;

    private ObjectPool bullet_Pool; //弾


    private void Start() {
        if (play_On_Awake) {
            Shoot();
        }
    }

    /// <summary>
    /// インスペクターで設定したショットを打つ
    /// </summary>
    public void Shoot() {
        bullet_Pool = ObjectPoolManager.Instance.Get_Pool(bullet);
        switch (kind) {
            case KIND.Scatter:  StartCoroutine("Scatter_Shoot"); break;
            case KIND.Spiral:   StartCoroutine("Spiral_Shoot"); break;
            default:            StartCoroutine("Shoot_Cor");    break;
        }
    }
    

    /// <summary>
    /// ショットを止める
    /// </summary>
    public void Stop_Shoot() {
        switch (kind) {
            case KIND.Scatter:  StopCoroutine("Scatter_Shoot"); break;
            case KIND.Spiral:   StopCoroutine("Spiral_Shoot");  break;
            default:            StopCoroutine("Shoot_Cor");     break;
        }
    }


    //ショット用のコルーチン
    private IEnumerator Shoot_Cor() {        
        List<GameObject> bullet_List = new List<GameObject>();        
        for (int i = 0; i < count; i++) {
            switch (kind) {
                case KIND.Odd:          bullet_List = Odd_Num_Shoot();  break;
                case KIND.Even:         bullet_List = Even_Num_Shoot(); break;
                case KIND.Diffusion:    bullet_List = Diffusion_Shoot(); break;
                case KIND.nWay:         bullet_List = nWay_Shoot();     break;
            }
            //弾の加速
            if (is_Acceleration) {
                StartCoroutine(Accelerat_Bullet_Cor(bullet_List, max_Speed));
            }
            //パラメータの変更
            if (is_Count_Up_Param) {
                num += num_Up;
                max_Speed += speed_Up;
                inter_Angle_Deg += inter_Angle_Up;
                center_Angle_Deg += center_Angle_Up;
            }

            yield return new WaitForSeconds(span);
        }
    }


    /// <summary>
    /// ばらまき弾
    /// </summary>
    /// <returns></returns>
    private IEnumerator Scatter_Shoot() {
        float angle = center_Angle_Deg;
        for(float t = 0; t < duration; t += 1 / shoot_Rate) {
            angle = center_Angle_Deg + Random.Range(-arc_Deg / 2, arc_Deg / 2);
            var b = Turn_Shoot_Bullet(angle);
            StartCoroutine(Accelerate_Bullet_Cor(b, max_Speed));
            yield return new WaitForSeconds(1 / shoot_Rate);
        }
    }


    /// <summary>
    /// 渦巻き弾
    /// </summary>
    /// <returns></returns>
    private IEnumerator Spiral_Shoot() {
        float angle = center_Angle_Deg;
        for(float t = 0; t < duration; t += 1 / shoot_Rate) {
            var b = Turn_Shoot_Bullet(angle);
            StartCoroutine(Accelerate_Bullet_Cor(b, max_Speed));
            angle += inter_Angle_Deg;
            yield return new WaitForSeconds(1 / shoot_Rate);
        }
    }


    //奇数段、偶数弾、全方位弾、nWay弾
    #region ShootFunction

    /// <summary>
    /// 奇数段
    /// </summary>
    public List<GameObject> Odd_Num_Shoot() {        
        List<GameObject> bullet_List = new List<GameObject>();        

        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player == null) {
            Debug.Log("Can't Find Player");
            return null;
        }

        AngleCalculater _angle = new AngleCalculater();
        center_Angle_Deg = _angle.Cal_Angle_Two_Points(transform.position, player.transform.position);
        float angle = center_Angle_Deg - num / 2 * inter_Angle_Deg;
        
        for (int i = 0; i < num; i++) {
            var b = Turn_Shoot_Bullet(angle);
            bullet_List.Add(b);
            angle += inter_Angle_Deg;
            Debug.Log(b);
        }

        return bullet_List;
    }


    /// <summary>
    /// 偶数弾
    /// </summary>
    public List<GameObject> Even_Num_Shoot() {
        List<GameObject> bullet_List = new List<GameObject>();

        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player == null) {
            Debug.Log("Can't Find Player");
            return null;
        }

        AngleCalculater _angle = new AngleCalculater();
        center_Angle_Deg = _angle.Cal_Angle_Two_Points(transform.position, player.transform.position);
        float angle = center_Angle_Deg - ((num - 1) * 0.5f * inter_Angle_Deg);

        for (int i = 0; i < num; i++) {
            var b = Turn_Shoot_Bullet(angle);
            bullet_List.Add(b);
            angle += inter_Angle_Deg;
        }

        return bullet_List;
    }


    /// <summary>
    /// 全方位弾
    /// </summary>
    public List<GameObject> Diffusion_Shoot() {
        List<GameObject> bullet_List = new List<GameObject>();

        //弾を円形に生成,発射
        for (int i = 0; i < num; i++) {
            float angle = i * 360f / num + center_Angle_Deg;
            GameObject bullet = Turn_Shoot_Bullet(angle);
            bullet_List.Add(bullet);
        }
        return bullet_List;
    }


    /// <summary>
    /// nWay弾
    /// </summary>
    public List<GameObject> nWay_Shoot() {
        List<GameObject> bullet_List = new List<GameObject>();

        float center;
        //偶数wayの場合
        if (num % 2 == 0) {
            center = num / 2 - 0.5f;
        }
        //奇数wayの場合
        else {
            center = num / 2;
        }
        //弾の生成、発射
        for (int i = 0; i < num; i++) {
            float angle = center_Angle_Deg + inter_Angle_Deg * (i - center);
            GameObject bullet = Turn_Shoot_Bullet(angle);
            bullet_List.Add(bullet);
        }
        return bullet_List;
    }

    #endregion


    /// <summary>
    /// 弾の生成、回転と発射
    /// </summary>
    public GameObject Turn_Shoot_Bullet(float angle_Deg) {
        var turn_Bullet = bullet_Pool.GetObject();                      //生成
        turn_Bullet.transform.SetParent(parent);                        //親オブジェクト        
        turn_Bullet.transform.position = transform.position;            //座標
        turn_Bullet.transform.rotation = new Quaternion(0, 0, 0, 0);
        turn_Bullet.transform.Rotate(new Vector3(0, 0, 1), angle_Deg);  //回転

        turn_Bullet.GetComponent<Rigidbody2D>().velocity = turn_Bullet.transform.right * max_Speed;     //初速
        if (lifeTime > 0) {
            Delete_Bullet(turn_Bullet, lifeTime);                       //寿命
        }
        return turn_Bullet;
    }


    //弾の消去
    private void Delete_Bullet(GameObject bullet, float lifeTime) {
        Bullet b = bullet.GetComponent<Bullet>();
        if (b != null) {
            b.Set_Inactive(lifeTime);
        }
        else {
            bullet.AddComponent<Bullet>().Set_Inactive(lifeTime);
        }
    }    


    //弾の加速(単体)
    public IEnumerator Accelerate_Bullet_Cor(GameObject bullet, float max_Speed) {
        Rigidbody2D bullet_Rigid = bullet.GetComponent<Rigidbody2D>();        
        
        float forward = max_Speed;
        float lateral = 0;
        for(float t = 0; t < lifeTime; t += Time.deltaTime) {
            if(!bullet.activeSelf) {
                yield break;
            }
            forward = max_Speed * velocity_Forward.Evaluate(t);                 //前方向
            lateral = max_Speed * velocity_Lateral.Evaluate(t);                 //横方向
            bullet_Rigid.velocity = bullet.transform.right * forward + bullet.transform.up * lateral;     //速度代入
            float dirVelocity = Mathf.Atan2(bullet_Rigid.velocity.y, bullet_Rigid.velocity.x) * Mathf.Rad2Deg;    //進行方向に回転
            bullet.transform.rotation = Quaternion.AngleAxis(dirVelocity, new Vector3(0, 0, 1));
            yield return new WaitForSeconds(0.016f);
        }
    }


    //弾の加速(List)
    public IEnumerator Accelerat_Bullet_Cor(List<GameObject> bullet_List, float max_Speed) {
        if (bullet_List[0].GetComponent<Rigidbody2D>() == null) {
            Debug.Log("Bullet Must Attached Rigidbody2D");
            yield break;
        }

        //Rigidbody2Dの取得
        List<Rigidbody2D> rigid_List = new List<Rigidbody2D>();
        for (int i = 0; i < bullet_List.Count; i++) {
            rigid_List.Add(bullet_List[i].GetComponent<Rigidbody2D>());
        }

        List<Rigidbody2D> remove_List = new List<Rigidbody2D>();
        //速度変更
        float forward = max_Speed;
        float lateral = 0;
        for (float t = 0; t < lifeTime; t += Time.deltaTime) {

            for (int i = 0; i < rigid_List.Count; i++) {
                //消えたものを除く
                if (!rigid_List[i].gameObject.activeSelf) {
                    remove_List.Add(rigid_List[i]);
                    continue;
                }             
                //速度の計算、代入
                forward = max_Speed * velocity_Forward.Evaluate(t); //前方向
                lateral = max_Speed * velocity_Lateral.Evaluate(t); //横方向                
                rigid_List[i].velocity = bullet_List[i].transform.right * forward + bullet_List[i].transform.up * lateral;    //速度代入
                float dirVelocity = Mathf.Atan2(rigid_List[i].velocity.y, rigid_List[i].velocity.x) * Mathf.Rad2Deg;    //進行方向に回転
                bullet_List[i].transform.rotation = Quaternion.AngleAxis(dirVelocity, new Vector3(0, 0, 1));
            }

            //消えたものを除く
            for(int i = 0; i < remove_List.Count; i++) {
                rigid_List.Remove(remove_List[i]);
                bullet_List.Remove(remove_List[i].gameObject);
            }
            remove_List.Clear();

            yield return new WaitForSeconds(0.015f);
        }
    }

}
