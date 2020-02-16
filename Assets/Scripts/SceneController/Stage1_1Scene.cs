using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1_1Scene : SingletonMonoBehaviour<Stage1_1Scene> {

    //ルーミアの状態
    public enum Rumia {
        not_find,
        find,
    }
    public Rumia rumia_State = Rumia.not_find;


    private void Start() {
        //オープニング
        if (SceneManagement.Instance.Is_First_Visit()) {
            GetComponent<OpeningMovie>().Start_Movie();            
        }
        
    }

}
