using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//デバッグ用にデータ等を変更できるようにする
public class DebugModeManager : SingletonMonoBehaviour<DebugModeManager> {
    
    
    //収集アイテムのデータを消す     
    public bool Delete_Collection_Data = false;

    //訪問済みシーンの消去
    public bool Delete_Visited_Scene_Date = false;

    //テストプレイ時のシーンを初回訪問にするかどうか
    public bool Is_First_Visit_Scene_In_Testplay = false;

    //自機のデータを消す    
    public bool Delete_Player_Data = false;
    //自機の初期データ
    public int Player_Life = 3;
    public int Player_Power = 0;

    //幽香戦のデータを消す        
    public bool Delete_Yuka_Data = false;               
    
}
