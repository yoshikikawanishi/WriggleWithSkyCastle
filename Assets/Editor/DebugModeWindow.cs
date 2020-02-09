﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class DebugModeWindow : EditorWindow {

    [MenuItem("Window/DebugModeWindow")]
    static void Open() {
        EditorWindow.GetWindow<DebugModeWindow>("DebugMode");
    }

    private void OnGUI() {
        var obj = (Resources.Load("CommonScripts") as GameObject).GetComponent<DebugModeManager>();

        EditorGUILayout.Space();

        obj.Delete_Collection_Data = EditorGUILayout.Toggle("Delete_Collection", obj.Delete_Collection_Data);
        EditorGUILayout.Space();

        obj.Delete_Visited_Scene_Date = EditorGUILayout.Toggle("Delete_Visited_Scene", obj.Delete_Visited_Scene_Date);
        EditorGUILayout.Space();

        obj.Is_First_Visit_Scene_In_Testplay = EditorGUILayout.Toggle("Play_Editor_In_First_Visit", obj.Is_First_Visit_Scene_In_Testplay);
        EditorGUILayout.Space();

        obj.Delete_Player_Data = EditorGUILayout.Toggle("Delete_Player_Data", obj.Delete_Player_Data);
        EditorGUILayout.Space();

        obj.Player_Life = EditorGUILayout.IntField("Player_Life", obj.Player_Life);
        obj.Player_Power = EditorGUILayout.IntField("Player_Power", obj.Player_Power);
        EditorGUILayout.Space();

        obj.Delete_Yuka_Data = EditorGUILayout.Toggle("Delete_Yuka_Tutorial_Data", obj.Delete_Yuka_Data);
        
    }    

}

