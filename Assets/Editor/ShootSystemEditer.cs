using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// ShootSystemのインスペクタ表示の設定
/// </summary>
[CustomEditor(typeof(ShootSystem))]
public class ShootSystemEditer : Editor {

    public override void OnInspectorGUI() {
        ShootSystem obj = target as ShootSystem;
        //基本設定
        obj.play_On_Awake = EditorGUILayout.Toggle("PlayOnAwake", obj.play_On_Awake);
        obj.bullet = (GameObject)EditorGUILayout.ObjectField("Bullet", obj.bullet, typeof(GameObject), false);
        obj.parent = (Transform)EditorGUILayout.ObjectField("Parent", obj.parent, typeof(Transform), true);
        obj.lifeTime = EditorGUILayout.FloatField("LifeTime", obj.lifeTime);        
        obj.max_Speed = EditorGUILayout.FloatField("MaxSpeed", obj.max_Speed);

        EditorGUILayout.Space();

        //その他パラメータ
        obj.kind = (ShootSystem.KIND)EditorGUILayout.EnumPopup("Kind", obj.kind);        
        obj.other_Param = EditorGUILayout.Foldout(obj.other_Param, "Param");
        if (obj.other_Param == true) {

            switch (obj.kind) {
                case ShootSystem.KIND.Odd:
                    obj.num = EditorGUILayout.IntField("Num", obj.num);
                    obj.inter_Angle_Deg = EditorGUILayout.FloatField("InterAngle_Deg", obj.inter_Angle_Deg);
                    obj.count = EditorGUILayout.IntField("Count", obj.count);
                    obj.span = EditorGUILayout.FloatField("Span", obj.span);
                    obj.is_Count_Up_Param = EditorGUILayout.Toggle("CountUpParam", obj.is_Count_Up_Param);
                    break;

                case ShootSystem.KIND.Even:
                    obj.num = EditorGUILayout.IntField("Num", obj.num);
                    obj.inter_Angle_Deg = EditorGUILayout.FloatField("InterAngle_Deg", obj.inter_Angle_Deg);
                    obj.count = EditorGUILayout.IntField("Count", obj.count);
                    obj.span = EditorGUILayout.FloatField("Span", obj.span);
                    obj.is_Count_Up_Param = EditorGUILayout.Toggle("CountUpParam", obj.is_Count_Up_Param);
                    break;

                case ShootSystem.KIND.Diffusion:
                    obj.num = EditorGUILayout.IntField("Num", obj.num);
                    obj.center_Angle_Deg = EditorGUILayout.FloatField("CenterAngle_Deg", obj.center_Angle_Deg);
                    obj.count = EditorGUILayout.IntField("Count", obj.count);
                    obj.span = EditorGUILayout.FloatField("Span", obj.span);
                    obj.is_Count_Up_Param = EditorGUILayout.Toggle("CountUpParam", obj.is_Count_Up_Param);
                    break;

                case ShootSystem.KIND.nWay:
                    obj.num = EditorGUILayout.IntField("Num", obj.num);
                    obj.inter_Angle_Deg = EditorGUILayout.FloatField("InterAngle_Deg", obj.inter_Angle_Deg);
                    obj.center_Angle_Deg = EditorGUILayout.FloatField("CenterAngle_Deg", obj.center_Angle_Deg);
                    obj.count = EditorGUILayout.IntField("Count", obj.count);
                    obj.span = EditorGUILayout.FloatField("Span", obj.span);
                    obj.is_Count_Up_Param = EditorGUILayout.Toggle("CountUpParam", obj.is_Count_Up_Param);
                    break;

                case ShootSystem.KIND.Scatter:
                    obj.shoot_Rate = EditorGUILayout.FloatField("Rate", obj.shoot_Rate);
                    obj.center_Angle_Deg = EditorGUILayout.FloatField("CenterAngle_Deg", obj.center_Angle_Deg);
                    obj.arc_Deg = EditorGUILayout.FloatField("Arc", obj.arc_Deg);
                    obj.duration = EditorGUILayout.FloatField("Duration", obj.duration);
                    obj.is_Count_Up_Param = false;
                    break;

                case ShootSystem.KIND.Spiral:
                    obj.shoot_Rate = EditorGUILayout.FloatField("Rate", obj.shoot_Rate);
                    obj.inter_Angle_Deg = EditorGUILayout.FloatField("InterAngle_Deg", obj.inter_Angle_Deg);
                    obj.center_Angle_Deg = EditorGUILayout.FloatField("CenterAngle_Deg", obj.center_Angle_Deg);
                    obj.duration = EditorGUILayout.FloatField("Duration", obj.duration);
                    obj.is_Count_Up_Param = false;
                    break;
            }
                        
            if (obj.is_Count_Up_Param) {
                obj.num_Up = EditorGUILayout.IntField("Num", obj.num_Up);
                obj.speed_Up = EditorGUILayout.FloatField("Speed", obj.speed_Up);
                obj.inter_Angle_Up = EditorGUILayout.FloatField("InterAngle", obj.inter_Angle_Up);
                obj.center_Angle_Up = EditorGUILayout.FloatField("CenterAngle", obj.center_Angle_Up);
            }
        }       

        EditorGUILayout.Space();
    
        //加速度
        obj.is_Acceleration = EditorGUILayout.Toggle("VelocityOverTime", obj.is_Acceleration);        
        if (obj.is_Acceleration) {
            if(obj.velocity_Forward == null) {
                obj.velocity_Forward = AnimationCurve.Linear(0, 0, obj.lifeTime, 1.0f);
                obj.velocity_Lateral = AnimationCurve.Linear(0, 0, obj.lifeTime, 1.0f);
            }
            obj.velocity_Forward = EditorGUILayout.CurveField("Forward", obj.velocity_Forward);
            obj.velocity_Lateral = EditorGUILayout.CurveField("Lateral", obj.velocity_Lateral);          
        }
    }    
    
}
