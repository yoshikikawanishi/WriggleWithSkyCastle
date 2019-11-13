using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamaged : MonoBehaviour {
    
    //無敵時間
    private float INVINCIBLE_TIME_LENGTH = 3.0f;    


    //被弾時の処理
    public IEnumerator Damaged() {          
        if (PlayerManager.Instance.Reduce_Life() == 0) {
            yield break;
        }

        PlayerBodyCollision body_Collision = GetComponentInChildren<PlayerBodyCollision>();

        Put_Out_Power(PlayerManager.Instance.Get_Power() / 8);      //パワーの減少
        StartCoroutine("Blink");                                    //点滅
        body_Collision.Become_Invincible();                         //無敵化
        Occure_Knock_Back();                                        //反動        
        yield return new WaitForSeconds(INVINCIBLE_TIME_LENGTH);    //無敵時間
        body_Collision.Release_Invincible();                        //戻す        
    }


    //点滅
    private IEnumerator Blink() {
        Renderer player_Renderer = GetComponent<Renderer>();
        float span = INVINCIBLE_TIME_LENGTH / 30;
        for (int i = 0; i < 15; i++) {
            player_Renderer.enabled = false;
            yield return new WaitForSeconds(span);
            player_Renderer.enabled = true;
            yield return new WaitForSeconds(span);
        }
    }


    //反動
    private void Occure_Knock_Back() {
        PlayerController _controller = GetComponent<PlayerController>();
        float force = _controller.is_Landing ? 200f : 100f;
        GetComponent<Rigidbody2D>().velocity = new Vector2(force * -transform.localScale.x, 100f);
    }


    //パワーの減少
    private void Put_Out_Power(int value) {
        PlayerManager.Instance.Set_Power(PlayerManager.Instance.Get_Power() - value);
        //アイテムの放出
        var power = Resources.Load("Object/Power") as GameObject;
        ObjectPool power_Pool = ObjectPoolManager.Instance.Get_Pool(power);
        for(int i = 0; i < value; i++) {
            var p = power_Pool.GetObject();
            p.transform.position = transform.position + new Vector3(0, 64f);
            Vector2 velocity = new Vector2(Random.Range(-15f, 15f) * i, Random.Range(300f, 500f));
            p.GetComponent<Rigidbody2D>().velocity = velocity;
        }
    }


    //MissZoneに当たったときの処理
    public void Miss() {
        PlayerManager.Instance.Set_Life(0);
    }
}
