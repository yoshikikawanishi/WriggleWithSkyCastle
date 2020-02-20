using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

    [SerializeField] private float surface_Height = -80f;

    private ObjectPool surface_Effect_Pool;
    private GameObject player;


	// Use this for initialization
	void Start () {
        //水面エフェクトのオブジェクトプール
        var effect = Resources.Load("Effect/WaterSurfaceEffect") as GameObject;
        ObjectPoolManager.Instance.Create_New_Pool(effect, 2);
        surface_Effect_Pool = ObjectPoolManager.Instance.Get_Pool(effect);
        //取得
        player = GameObject.FindWithTag("PlayerTag");
    }
	

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag" && Is_Player_In_Surface()) {
            Play_Surface_Effect(collision.transform.position);              
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "PlayerBodyTag" && Is_Player_In_Surface()) {
            Play_Surface_Effect(collision.transform.position);            
        }
    }


    //水面の出入りでエフェクトを出す
    private void Play_Surface_Effect(Vector2 pos) {
        var effect = surface_Effect_Pool.GetObject();
        effect.transform.position = new Vector3(pos.x, surface_Height);
        
    }

    //自機が水面の高さにいるかどうか、誤差16まで許す
    private bool Is_Player_In_Surface() {
        if (player == null)
            return false;

        if(Mathf.Abs(player.transform.position.y - surface_Height) < 16) {
            return true;
        }
        return false;
    }

}
