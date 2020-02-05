using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minoriko : MonoBehaviour {

    

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void Shoot_Normal() {
        GameObject shoot_Obj = Instantiate(transform.GetChild(0).gameObject);
        shoot_Obj.transform.position = transform.position;
        shoot_Obj.SetActive(true);
        Destroy(shoot_Obj, 2.0f);
    }
}
