using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMotion : MonoBehaviour {

    [SerializeField] private float amplitude = 16f;
    [SerializeField] private float angular_Speed = 10f;
    [SerializeField] private float start_Angular = 0;

    private float angle = 0;
    private float center_Pos;


	// Use this for initialization
	void Start () {
        angle = start_Angular;
        center_Pos = transform.position.y;
	}

	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, center_Pos + Mathf.Sin(Mathf.Deg2Rad * angle));
        angle = (angle + angular_Speed) % 360f;
	}
}
