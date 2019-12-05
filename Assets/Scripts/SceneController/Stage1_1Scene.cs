using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1_1Scene : SingletonMonoBehaviour<Stage1_1Scene> {

    //ルーミアの状態
    public enum Rumia {
        not_find,
        find,
        delete
    }
    public Rumia rumia_State = Rumia.not_find;


    private void Start() {
        //フェードイン
        if (SceneManagement.Instance.Is_First_Visit()) {
            FadeInOut.Instance.Start_Fade_In(new Color(0, 0, 0), 0.02f);
        }
    }

}
