using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    //オブジェクトと結びつける
    public InputField inputField;
    public Text text;

    public GameObject canvas;//キャンバス
    public GameObject gametext;

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
        //オブジェクトを表示する
        gametext.gameObject.SetActive(true);
    }

    //テキストボックスへの入力が終わった時に呼び出す
    public void EndEdit()
    {
        if (GameObject.Find("TextBox").GetComponent<InputField>().text != "") {
            //テキストがあればプレハブからオブジェクト生成
            GameObject prefab = (GameObject)Instantiate(gametext);
            prefab.transform.SetParent(canvas.transform, false);
        }
    }

    //送信ボタンを押した時
    public void Send()
    {
        //インプットフィールドの中身を消す
        GameObject.Find("TextBox").GetComponent<InputField>().text = "";
    }
}
