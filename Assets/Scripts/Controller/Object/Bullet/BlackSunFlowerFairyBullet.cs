using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackSunFlowerFairyBullet : MonoBehaviour {

    private void OnEnable() {
        StartCoroutine("Shoot_Cor");
    }

    private IEnumerator Shoot_Cor() {
        yield return null;        
        ShootSystem _shoot = GetComponent<ShootSystem>();
        _shoot.center_Angle_Deg = transform.localEulerAngles.z;
        _shoot.Shoot();
    }

}
