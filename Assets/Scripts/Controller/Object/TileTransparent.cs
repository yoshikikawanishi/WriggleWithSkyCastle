using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 自機が通常時または飛行時に半透明になり、当たり判定を消すTilemapオブジェクトにアタッチ
/// </summary>
public class TileTransparent : MonoBehaviour {

    private enum PlayerState {
        normal,
        fly,
    }
    [SerializeField] PlayerState appear_State;

    //コンポーネント
    private PlayerController player_Controller;
    private Tilemap _tilemap;
    private TilemapCollider2D _collider;

	
    // Use this for initialization
	void Start () {
        //取得
        player_Controller = GameObject.FindWithTag("PlayerTag").GetComponent<PlayerController>();
        _tilemap = GetComponent<Tilemap>();
        _collider = GetComponent<TilemapCollider2D>();
	}


    private void Update() {
        if(player_Controller == null) 
            return;
        
        //飛行時
        if(player_Controller.Get_Is_Ride_Beetle()) {           
            //具現化
            if (appear_State == PlayerState.fly) {
                Become_Appearance();
            }
            //透明化
            else {
                Become_Transparent();
            }
        }
        //地上時
        else {            
            //具現化
            if (appear_State == PlayerState.normal) {
                Become_Appearance();
            }
            //透明化
            else {
                Become_Transparent();
            }
        }
    }


    //透明になる
    private void Become_Transparent() {
        if (_collider.enabled) {
            _tilemap.color = new Color(1, 1, 1, 0.4f);
            _collider.enabled = false;
        }
    }

    //具現化する
    private void Become_Appearance() {
        if (!_collider.enabled) {
            _tilemap.color = new Color(1, 1, 1, 1);
            _collider.enabled = true;
        }
    }

}
