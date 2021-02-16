using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour
{
    public Text text;   //テキスト格納用
    bool Rec_or_Stop;   //音声認識中かそうでないかのbool文
    // Start is called before the first frame update
    void Start()
    {
        Rec_or_Stop = true; //音声認識前
        text.text = "Rec";  //Recの表示
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Onclick()
    {
        if(Rec_or_Stop)         //音声認識前に押されたら
        {
            text.text = "Stop"; //Stopの表示
            Rec_or_Stop = false;//音声認識中
        }
        else                    //音声認識中に押されたら
        {
            text.text = "Rec";  //Recの表示
            Rec_or_Stop = true; //音声認識前
        }
        
    }
}
