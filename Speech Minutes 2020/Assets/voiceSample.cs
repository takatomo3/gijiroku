using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class voiceSample : MonoBehaviour
{
    private PXCMSession session;
    private PXCMAudioSource source;
    private PXCMSpeechRecognition sr;
    private PXCMSpeechRecognition.Handler handler;
    bool Record = true;
    public string outputtext;
    string filePath;


    public void Start()
    {
        filePath = Application.dataPath + @"\LogData.txt";
        File.CreateText(filePath);

        //ブラックボックス
            session = PXCMSession.CreateInstance();
            source = session.CreateAudioSource();

            PXCMAudioSource.DeviceInfo dinfo = null;

            //デバイスを検出して出力
            source.QueryDeviceInfo(0, out dinfo);
            source.SetDevice(dinfo);
            Debug.Log(dinfo.name);

            session.CreateImpl<PXCMSpeechRecognition>(out sr);

            PXCMSpeechRecognition.ProfileInfo pinfo;
            sr.QueryProfile(out pinfo);
            pinfo.language = PXCMSpeechRecognition.LanguageType.LANGUAGE_JP_JAPANESE;
            sr.SetProfile(pinfo);

            handler = new PXCMSpeechRecognition.Handler();
            //handler.onRecognition = (x) => Debug.Log(x.scores[0].sentence);
            sr.SetDictation();
            //sr.StartRec(source, handler);
        //ブラックボックスここまで

    }

    //データの出力
    void Dataoutput(string text)
    {
        //StreamWriter型のswを宣言
        StreamWriter sw = new StreamWriter(filePath,true);
        //改行
        sw.Write(text+"\n");
        sw.Close();
        //ログの書き出し
        Debug.Log(text);
        outputtext = text;
        

    }

    //Recordボタンを押すと呼び出されるメソッド
    public void SetRecord()
    {
        //Recordがtrueなら(最初に押されたら)
        if (Record)
        {

            handler.onRecognition = (x) => Dataoutput(x.scores[0].sentence);    //わかりません
            sr.StartRec(source, handler);                                       //音声認識の開始
            Record = false;                                                     //Recordをfalseにする
            Debug.Log("Record True");                                           //デバッグログ
        }
        //Recordがfalseなら(音声認識中に押されたら)
        else
        {
            sr.StopRec();               //音声認識の終了
            Record = true;              //Recordをtrueにする
            Debug.Log("Record False");  //デバッグログ
        }
    }

    //ブラックボックス
    void OnDisable()
    {
        if (sr != null)
        {
            sr.StopRec();
            sr.Dispose();
        }

        if (session != null)
            session.Dispose();
    }
}
