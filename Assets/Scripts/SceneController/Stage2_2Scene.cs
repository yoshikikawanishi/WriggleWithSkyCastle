﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_2Scene : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //フェードイン
        if (SceneManagement.Instance.Is_First_Visit()) {
            FadeInOut.Instance.Start_Fade_In(new Color(0, 0, 0), 0.02f);
        }
    }
	
}
