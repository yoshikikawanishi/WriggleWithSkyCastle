using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YukaAttack : MonoBehaviour {
    

    private bool start_Battle = false;


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (start_Battle) {

        }
    }


    //戦闘開始
    public void Start_Battle() {
        start_Battle = true;

    }
    
}
