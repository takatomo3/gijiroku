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
    string LogDataFilePath = @"\LogData.txt";       //Assets\以下の音声ファイルの書き込み先のファイル指定


    public void Start()
    {
        //LogData i .txt の初期化
        for(int i = 0; i < 4; i++)
        {
            //filePathのパス指定
            FilePathSelect(i);
            File.CreateText(filePath);
        }
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

    //filePathのパス指定
   void FilePathSelect(int number)
    {
        switch (number)
        {
            case 0:
                LogDataFilePath = @"\LogData0.txt";
                filePath = Application.dataPath + LogDataFilePath;
                break;
            case 1:
                LogDataFilePath = @"\LogData1.txt";
                filePath = Application.dataPath + LogDataFilePath;
                break;
            case 2:
                LogDataFilePath = @"\LogData2.txt";
                filePath = Application.dataPath + LogDataFilePath;
                break;
            case 3:
                LogDataFilePath = @"\LogData3.txt";
                filePath = Application.dataPath + LogDataFilePath;
                break;
            default:
                break;
        }
    }

    //話題ボタンが押されると呼び出されるメソッド
    public void WadaiButton0()
    {
        FilePathSelect(0);
        Debug.Log("話題0が押されました");
    }
    public void WadaiButton1()
    {
        FilePathSelect(1);
        Debug.Log("話題1が押されました");
    }
    public void WadaiButton2()
    {
        FilePathSelect(2);
        Debug.Log("話題2が押されました");
    }
    public void WadaiButton3()
    {
        FilePathSelect(3);
        Debug.Log("話題3が押されました");
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

    /*
    void Update()
    {
        filePath = Application.dataPath + LogDataFilePath;
        File.CreateText(filePath);
    }
    */

}
