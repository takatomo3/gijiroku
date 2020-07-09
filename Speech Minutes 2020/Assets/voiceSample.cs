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
    string filePath;
    public GameObject ButtonText;


    public void Start()
    {
        filePath = Application.dataPath + @"\LogData.txt";
        File.CreateText(filePath);

            //インスタンスの生成
            session = PXCMSession.CreateInstance();
            //音声データの入力
            source = session.CreateAudioSource();
           
            PXCMAudioSource.DeviceInfo dinfo = null;

            //デバイスを検出して出力
            source.QueryDeviceInfo(0, out dinfo);
            source.SetDevice(dinfo);
            Debug.Log(dinfo.name);

            //音声認識
            session.CreateImpl<PXCMSpeechRecognition>(out sr);

            //音声認識の初期設定
            PXCMSpeechRecognition.ProfileInfo pinfo;
            sr.QueryProfile(out pinfo);
            pinfo.language = PXCMSpeechRecognition.LanguageType.LANGUAGE_JP_JAPANESE;
            sr.SetProfile(pinfo);

            //handlerにメソッドを渡す工程
            handler = new PXCMSpeechRecognition.Handler();
            handler.onRecognition = (x) => Dataoutput(x.scores[0].sentence, x.duration);
            sr.SetDictation();
    }

    //データの出力
    void Dataoutput(string text , int duration)
    {
        using (StreamWriter sw = new StreamWriter(filePath, true))
        {
            sw.WriteLine(text +" , "+ duration.ToString()); //テキストファイルに音声認識結果テキストと、音声認識時間(ms)を書きこみ
        }
        //ログの書き出し
        Debug.Log(text + " , " + duration.ToString());
    }


    //Recordボタンを押すと呼び出されるメソッド
    public void SetRecord()
    {
        
        //Recordがtrueなら(最初に押されたら)
        if (Record)
        {
            //handler.onRecognition = (x) => Dataoutput(x.scores[0].sentence);    //わかりません
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

    //バグ防止
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
