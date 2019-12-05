using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShootSystem))]
public class ShootSystemEditer : Editor {

    public override void OnInspectorGUI() {
        ShootSystem obj = target as ShootSystem;
        //基本設定
        obj.bullet = (GameObject)EditorGUILayout.ObjectField("Bullet", obj.bullet, typeof(GameObject), false);
        obj.parent = (GameObject)EditorGUILayout.ObjectField("Parent", obj.parent, typeof(GameObject), true);      

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
                    break;
                case ShootSystem.KIND.Even:
                    obj.num = EditorGUILayout.IntField("Num", obj.num);
                    obj.inter_Angle_Deg = EditorGUILayout.FloatField("InterAngle_Deg", obj.inter_Angle_Deg);
                    obj.count = EditorGUILayout.IntField("Count", obj.count);
                    obj.span = EditorGUILayout.FloatField("Span", obj.span);
                    break;
                case ShootSystem.KIND.Diffusion:
                    obj.num = EditorGUILayout.IntField("Num", obj.num);
                    obj.center_Angle_Deg = EditorGUILayout.FloatField("CenterAngle_Deg", obj.center_Angle_Deg);
                    obj.count = EditorGUILayout.IntField("Count", obj.count);
                    obj.span = EditorGUILayout.FloatField("Span", obj.span);
                    break;
                case ShootSystem.KIND.nWay:
                    obj.num = EditorGUILayout.IntField("Num", obj.num);
                    obj.inter_Angle_Deg = EditorGUILayout.FloatField("InterAngle_Deg", obj.inter_Angle_Deg);
                    obj.center_Angle_Deg = EditorGUILayout.FloatField("CenterAngle_Deg", obj.center_Angle_Deg);
                    obj.count = EditorGUILayout.IntField("Count", obj.count);
                    obj.span = EditorGUILayout.FloatField("Span", obj.span);
                    break;
                case ShootSystem.KIND.Scatter:
                    obj.shoot_Rate = EditorGUILayout.FloatField("Rate", obj.shoot_Rate);
                    obj.center_Angle_Deg = EditorGUILayout.FloatField("CenterAngle_Deg", obj.center_Angle_Deg);
                    obj.arc_Deg = EditorGUILayout.FloatField("Arc", obj.arc_Deg);
                    obj.duration = EditorGUILayout.FloatField("Duration", obj.duration);
                    break;
                case ShootSystem.KIND.Spiral:
                    obj.shoot_Rate = EditorGUILayout.FloatField("Rate", obj.shoot_Rate);
                    obj.inter_Angle_Deg = EditorGUILayout.FloatField("InterAngle_Deg", obj.inter_Angle_Deg);
                    obj.duration = EditorGUILayout.FloatField("Duration", obj.duration);
                    break;
            }

        }

        EditorGUILayout.Space();

        //速度
        obj.max_Speed = EditorGUILayout.FloatField("MaxSpeed", obj.max_Speed);
        obj.velocity_Over_Lifetime = EditorGUILayout.Foldout(obj.velocity_Over_Lifetime, "VelocityOverTime");
        if (obj.velocity_Over_Lifetime == true) {
            obj.velocity_Forward = EditorGUILayout.CurveField("Forward", obj.velocity_Forward);
            obj.velocity_Lateral = EditorGUILayout.CurveField("Lateral", obj.velocity_Lateral);
        }
    }    
}
