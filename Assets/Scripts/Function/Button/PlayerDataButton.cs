using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MBLDefine;

public class PlayerDataButton : MonoBehaviour {

    public Button first_Select_Button;
    public Text item_Comment_Text;


	// Use this for initialization
	void Start () {
        first_Select_Button.Select();	
	}

    public void Back_Title_Button() {
        if (InputManager.Instance.GetKeyDown(Key.Jump)) {
            SceneManager.LoadScene("TitleScene");
        }
    }


    public void Display_Item_Comment(string item_Name) {
        if (item_Dic.ContainsKey(item_Name))
            item_Comment_Text.text = item_Dic[item_Name];
        else
            Debug.Log("There is no item comment");
    }



    /*---------------------------------------アイテムとそのコメント----------------------------------------*/
    private Dictionary<string, string> item_Dic = new Dictionary<string, string>() {
        { "Rumia", "ルーミアのリボン" },
    };
}
