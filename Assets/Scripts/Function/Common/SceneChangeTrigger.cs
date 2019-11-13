using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTrigger : MonoBehaviour {

    public string next_Scene;

    public enum Change_Efect {
        non,
        fade_Out
    }
    public Change_Efect change_Effect_Type;


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag") {
            StartCoroutine("Change_Scene_Cor");
        }
    }

    
    //シーン遷移用コルーチン
    private IEnumerator Change_Scene_Cor() {
        //遷移前のエフェクト
        switch (change_Effect_Type) {
            case Change_Efect.non:
                break;
            case Change_Efect.fade_Out:
                FadeInOut.Instance.Start_Fade_Out(new Color(0, 0, 0), 0.05f);
                yield return new WaitForSeconds(1.0f);
                break;
        }
        SceneManager.LoadScene(next_Scene);
    }

}
