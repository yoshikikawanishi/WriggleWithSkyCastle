using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackSunFlowerFairyBullet : MonoBehaviour {

    [SerializeField] private GameObject main_Purple_Bullet;

    private ShootSystem _shoot;


    private void Awake() {
        _shoot = GetComponent<ShootSystem>();
    }

    private void OnEnable() {
        StartCoroutine("Shoot_Cor");
        this.enabled = true;
    }

    private void Update() {
        if (!main_Purple_Bullet.activeSelf) {
            _shoot.Stop_Shoot();
            this.enabled = false;
        }
    }

    private IEnumerator Shoot_Cor() {
        yield return null;                
        _shoot.center_Angle_Deg = transform.parent.localEulerAngles.z;
        _shoot.Shoot();
    }

}
