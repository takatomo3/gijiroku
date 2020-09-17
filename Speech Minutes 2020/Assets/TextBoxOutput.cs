using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxOutput : MonoBehaviour
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
        gametext.gameObject.SetActive(true);
    }

    public void EndEdit()
    {
        GameObject prefab = (GameObject)Instantiate(gametext);
        prefab.transform.SetParent(canvas.transform, false);
    }
}
