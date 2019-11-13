﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : SingletonMonoBehaviour<SceneManagement> {

    private bool is_First_Visit = true;


    // Use this for initialization
    void Start() {
        //シーン読み込みのデリケート
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    //訪れたシーンを保存
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        is_First_Visit = true;
        if (Has_Visited(scene.name)) {
            is_First_Visit = false;
        }
        Save_Visit_Scene(scene.name);       
    }


    //訪れたシーンの保存
    private void Save_Visit_Scene(string scene) {            
        string filePath = Application.dataPath + @"\StreamingAssets\VisitedSceneList.txt";
        TextFileReader text = new TextFileReader();
        text.Read_Text_File_Path(filePath);
        
        if (Has_Visited(scene)) {
            return;
        }
        
        StreamWriter sw = new StreamWriter(filePath, true);
        sw.Write("\n" + scene);

        sw.Flush();
        sw.Close();
    }


    //進行度の消去
    public void Delete_Visit_Scene() {
        string filePath = Application.dataPath + @"\StreamingAssets\VisitedSceneList.txt";
        TextFileReader text = new TextFileReader();
        text.Read_Text_File_Path(filePath);
        

        StreamWriter sw_Clear = new StreamWriter(filePath, false);
        sw_Clear.Write("#SceneName");

        sw_Clear.Flush();
        sw_Clear.Close();
    }


    //引数シーンに訪れたことがあるか
    public bool Has_Visited(string scene) {
        string filePath = Application.dataPath + @"\StreamingAssets\VisitedSceneList.txt";
        TextFileReader text = new TextFileReader();
        text.Read_Text_File_Path(filePath);

        for(int i = 1; i < text.rowLength; i++) {
            if(text.textWords[i, 0] == scene) {
                return true;
            }
        }

        return false;
    }


    //現在のシーンが初訪問かどうか
    public bool Is_First_Visit() {
        return is_First_Visit;
    }


    //ゲームシーン
    private Dictionary<string, bool> Is_Game_Scene_Dic = new Dictionary<string, bool>() {
        { "TitleScene", false },
        { "Stage1_1Scene", true },
        { "Stage1_BossScene", true }
    };

    //ゲームシーンかどうか
    public bool Is_Game_Scene(string scene) {
        if (Is_Game_Scene_Dic.ContainsKey(scene)) {
            return Is_Game_Scene_Dic[scene];
        }
        return false;
    }
    

}
