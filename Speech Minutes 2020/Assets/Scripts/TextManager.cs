using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    //オブジェクトと結びつける
    public InputField inputField;
    public Text text;
    public Text display;

    public GameObject canvas;//キャンバス
    public GameObject FusenPanel;

    // Start is called before the first frame update
    void Start()
    {
        //Componentを扱えるようにする
        inputField = inputField.GetComponent<InputField>();
        text = text.GetComponent<Text>();
    }

    public void InputText()
    {
        //テキストにinputFieldの内容を反映
        text.text = inputField.text;
        display.text = inputField.text;
        //オブジェクトを表示する
      //  gametext.gameObject.SetActive(true);
    }
    int resetcunter = 0;
    public RectTransform input_rectTransform = null;
    public RectTransform display_rectTransform = null;
    //テキストボックスへの入力が終わった時に呼び出す
    public void EndEdit()
    { 
        if (GameObject.Find("TextBox").GetComponent<InputField>().text != "") {
            //テキストがあればプレハブからオブジェクト生成
            FusenPanel.gameObject.SetActive(true);
            GameObject prefab = (GameObject)Instantiate(FusenPanel);
            prefab.transform.SetParent(canvas.transform, false);
            //int px = Random.Range(30, -35);
            //int py = Random.Range(19, -25);
           
            if (resetcunter<4)
            {
                resetcunter += 1;
                input_rectTransform.position += new Vector3(10, -20, 0f);
                display_rectTransform.position = input_rectTransform.position;
            }else if(resetcunter>=4)
            {
                resetcunter = 0;
                input_rectTransform.position += new Vector3(-40, 80, 0f); ;
                display_rectTransform.position = input_rectTransform.position;
            }
        }
    }

   /* public void Update()
    {
        EventSystem ev = EventSystem.current;
        if (ev.alreadySelecting)
        {
            Debug.Log("何かを選択しています");
        }
    }*/
    //送信ボタンを押した時
    public void Send()
    {
        //インプットフィールドの中身を消す
        GameObject.Find("TextBox").GetComponent<InputField>().text = "";
        FusenPanel.gameObject.SetActive(false);
    }
}
