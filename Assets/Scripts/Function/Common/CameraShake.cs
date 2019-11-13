using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//画面を揺らすスクリプト
public class CameraShake : MonoBehaviour {

    public void Shake(float duration, float magnitude, bool is_Fixed_Camera) {
        StartCoroutine(DoShake(duration, magnitude, is_Fixed_Camera));
    }

    //カメラ揺らす
    private IEnumerator DoShake(float duration, float magnitude, bool is_Fixed) {
        GameObject main_Camera = GameObject.FindWithTag("MainCamera");
        var pos = main_Camera.transform.localPosition;
        var default_Pos = main_Camera.transform.localPosition;
        var elapsed = 0f;

        //固定カメラの時
        if (is_Fixed) {
            while (elapsed < duration) {
                var x = pos.x + Random.Range(-1f, 1f) * magnitude;
                var y = pos.y + Random.Range(-1f, 1f) * magnitude;

                main_Camera.transform.localPosition = new Vector3(x, y, pos.z);

                elapsed += Time.deltaTime;

                yield return new WaitForSeconds(0.016f);
            }
            main_Camera.transform.position = pos;
        }
        
        //カメラが動くとき
        else {
            while (elapsed < duration) {
                pos = main_Camera.transform.localPosition;
                var x = pos.x + Random.Range(-1f, 1f) * magnitude;
                var y = default_Pos.y + Random.Range(-1f, 1f) * magnitude;

                main_Camera.transform.localPosition = new Vector3(x, y, pos.z);

                elapsed += Time.deltaTime;

                yield return new WaitForSeconds(0.016f);
            }
            main_Camera.transform.position = new Vector3(main_Camera.transform.position.x, 0, default_Pos.z);
        }
    }


    //揺らしている間、カメラコントローラーを切る
    public void Remove_Camera_Controller(float duration) {
        StartCoroutine(DoRemove_Camera_Controller(duration));
    }

    private IEnumerator DoRemove_Camera_Controller(float duration) {
        GameObject main_Camera = GameObject.FindWithTag("MainCamera");
        main_Camera.GetComponent<CameraController>().enabled = false;
        yield return new WaitForSeconds(duration);
        main_Camera.GetComponent<CameraController>().enabled = true;
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
