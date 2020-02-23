﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GrassGround : MonoBehaviour {

    private List<string> player_Attack_Tags = new List<string> {
        "PlayerAttackTag",
        "PlayerButterflyAttackTag",
        "PlayerSpiderAttackTag",
        "PlayerChargeAttackTag",
    };

    private Tilemap _tilemap;
    private PlayerAttackCollision player_Attack_Collision;
    private GameObject leaf_Effect_Prefab;

    readonly private Vector2Int CELL_SIZE = new Vector2Int(32, 32);


	// Use this for initialization
	void Start () {
        //取得
        _tilemap = GetComponent<Tilemap>();
        player_Attack_Collision = GameObject.FindWithTag("PlayerTag").GetComponentInChildren<PlayerAttackCollision>();
        //エフェクトのオブジェクトプール
        leaf_Effect_Prefab = Resources.Load("Effect/LeafEffect") as GameObject;
        ObjectPoolManager.Instance.Create_New_Pool(leaf_Effect_Prefab, 4);
	}


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if (player_Attack_Collision == null)
            return;

        foreach(string tag in player_Attack_Tags) {
            if(collision.tag == tag) {
                //攻撃の範囲を取得
                Vector2 left_Bottom = player_Attack_Collision.Get_Collision_Range()[0];
                Vector2 right_Top = player_Attack_Collision.Get_Collision_Range()[1];
                //範囲内のタイルを消す
                Search_Tile_And_Delete(left_Bottom, right_Top);
            }
        }  
    }


    //引数範囲内のタイルを消す
    private void Search_Tile_And_Delete(Vector2 left_Bottom, Vector2 right_Top) {        

        //範囲内に含まれる一番左下のセルの番号        
        /*
                ...｜   ｜   ｜   ｜.....
           Pos    -64  -32    0   32
           Index     -2   -1    0    1
         */
        Vector2Int left_Bottom_Cell = new Vector2Int(
            (int)(left_Bottom.x / CELL_SIZE.x),
            (int)(left_Bottom.y / CELL_SIZE.y)
            );
        if (left_Bottom.x < 0)
            left_Bottom_Cell += new Vector2Int(-1, 0);
        if (left_Bottom.y < 0)
            left_Bottom_Cell += new Vector2Int(0, -1);
        //範囲内に含まれる一番右上のセルの番号
        Vector2Int right_Top_Cell = new Vector2Int(
            (int)(right_Top.x / CELL_SIZE.x),
            (int)(right_Top.y / CELL_SIZE.y)
            );
        if (right_Top.x < 0)
            right_Top_Cell += new Vector2Int(-1, 0);
        if (right_Top.y < 0)
            right_Top_Cell += new Vector2Int(0, -1);        

        //範囲内のタイルを消す
        TileBase tile_tmp;
        for(int x = left_Bottom_Cell.x; x <= right_Top_Cell.x; x++) {
            for (int y = left_Bottom_Cell.y; y <= right_Top_Cell.y; y++) {

                tile_tmp = _tilemap.GetTile(new Vector3Int(x, y, 0));
                if (tile_tmp == null)
                    continue;

                _tilemap.SetTile(new Vector3Int(x, y, 0), null);                            //消す
                Play_Delete_Effect(_tilemap.GetCellCenterWorld(new Vector3Int(x, y, 0)));   //エフェクト出す
            }
        }
    }    


    //消滅エフェクト
    private void Play_Delete_Effect(Vector3 pos) {
        var effect = ObjectPoolManager.Instance.Get_Pool(leaf_Effect_Prefab).GetObject();
        effect.transform.position = pos;
        ObjectPoolManager.Instance.Set_Inactive(effect, 2.5f);
    }
}
