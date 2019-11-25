using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//画面を揺らすスクリプト
public class CameraShake : MonoBehaviour {

    public void Shake(float duration, float magnitude) {
        StartCoroutine(DoShake(duration, magnitude));
    }

    //カメラ揺らす
    private IEnumerator DoShake(float duration, float magnitude) {

        GameObject main_Camera = GameObject.FindWithTag("MainCamera");
        var pos = main_Camera.transform.localPosition;        
        var elapsed = 0f;

        Remove_Camera_Controller(duration);

        while (elapsed < duration) {
            var x = pos.x + Random.Range(-1f, 1f) * magnitude;
            var y = pos.y + Random.Range(-1f, 1f) * magnitude;

            main_Camera.transform.localPosition = new Vector3(x, y, pos.z) * Time.timeScale;

            elapsed += Time.deltaTime;

            yield return null;
        }

        main_Camera.transform.position = pos;

    }


    //揺らしている間、カメラコントローラーを切る
    public void Remove_Camera_Controller(float duration) {
        StartCoroutine(DoRemove_Camera_Controller(duration));
    }

    private IEnumerator DoRemove_Camera_Controller(float duration) {
        CameraController camera_Controller = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
        if (camera_Controller == null)
            yield break;
        
        camera_Controller.enabled = false;
        yield return new WaitForSeconds(duration);
        camera_Controller.enabled = true;
    }

}



/*使用例
    public CameraShake shake;

    private void Update()
    {
        if ( Input.GetKeyDown( KeyCode.Z ) )
        {
            shake.Shake( 0.25f, 0.1f );
        }
    } 
     */
