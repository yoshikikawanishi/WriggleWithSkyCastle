using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("Test1", 3.0f);
	}
	

	private void Test1() {
        GetComponent<ShootSystem>().Shoot();
    }

}
