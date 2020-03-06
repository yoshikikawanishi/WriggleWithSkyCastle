using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//デバッグ用にデータ等を変更できるようにする
public class DebugModeManager : SingletonMonoBehaviour<DebugModeManager> {
    
    
    //収集アイテムのデータを消す   
    [SerializeField]
    public bool Delete_Collection_Data;

    //訪問済みシーンの消去
    [SerializeField]
    public bool Delete_Visited_Scene_Date;

    //テストプレイ時のシーンを初回訪問にするかどうか
    [SerializeField]
    public bool Is_First_Visit_Scene_In_Testplay;

    //自機のデータを消す    
    [SerializeField]
    public bool Delete_Player_Data;
    //自機の初期データ
    [SerializeField]
    public int Player_Life = 9;
    [SerializeField]
    public int Player_Power = 64;

    //幽香戦のデータを消す        
    [SerializeField]
    public bool Delete_Yuka_Data;               
    
}
