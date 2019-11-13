using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMotion : MonoBehaviour {

    [SerializeField] private float rotate_Speed = 0.1f;
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, 0, rotate_Speed * Time.timeScale));
	}
}
