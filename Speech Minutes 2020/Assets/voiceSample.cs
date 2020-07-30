using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;


public class voiceSample : MonoBehaviour
{
    private PXCMSession session;
    private PXCMAudioSource source;
    private PXCMSpeechRecognition sr;
    private PXCMSpeechRecognition.Handler handler;
    bool Record = true;
    string filePath;
    string LogDataFilePath = @"\LogData.txt";       //Assets\以下の音声ファイルの書き込み先のファイル指定
    public Text text;
    public GameObject[] Button;
    int NowBottonPushed = -1;


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

        text.text = "話題未選択";
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
                LogDataFilePath = @"\LogDatas\LogData0.txt";
                filePath = Application.dataPath + LogDataFilePath;
                text.text = Button[number].GetComponentInChildren<Text>().text + "を選択中";
                break;
            case 1:
                LogDataFilePath = @"\LogDatas\LogData1.txt";
                filePath = Application.dataPath + LogDataFilePath;
                text.text = Button[number].GetComponentInChildren<Text>().text + "を選択中";
                break;
            case 2:
                LogDataFilePath = @"\LogDatas\LogData2.txt";
                filePath = Application.dataPath + LogDataFilePath;
                text.text = Button[number].GetComponentInChildren<Text>().text + "を選択中";
                break;
            case 3:
                LogDataFilePath = @"\LogDatas\LogData3.txt";
                filePath = Application.dataPath + LogDataFilePath;
                text.text = Button[number].GetComponentInChildren<Text>().text + "を選択中";
                break;
            case 4:
                LogDataFilePath = @"\LogDatas\LogData4.txt";
                filePath = Application.dataPath + LogDataFilePath;
                text.text = Button[number].GetComponentInChildren<Text>().text + "を選択中";
                break;
            case 5:
                LogDataFilePath = @"\LogDatas\LogData5.txt";
                filePath = Application.dataPath + LogDataFilePath;
                text.text = Button[number].GetComponentInChildren<Text>().text + "を選択中";
                break;
            case 6:
                LogDataFilePath = @"\LogDatas\LogData6.txt";
                filePath = Application.dataPath + LogDataFilePath;
                text.text = Button[number].GetComponentInChildren<Text>().text + "を選択中";
                break;
            case 7:
                LogDataFilePath = @"\LogDatas\LogData7.txt";
                filePath = Application.dataPath + LogDataFilePath;
                text.text = Button[number].GetComponentInChildren<Text>().text + "を選択中";
                break;
            default:
                LogDataFilePath = @"\LogDatas\LogData.txt";
                filePath = Application.dataPath + LogDataFilePath;
                text.text = "話題未選択";
                break;
        }
    }

    //話題ボタンが押されると呼び出されるメソッド
    public void WadaiButton0()
    {
        if(NowBottonPushed != 0)
        {
            FilePathSelect(0);
            Debug.Log("話題0が押されました");
            NowBottonPushed = 0;
        }
        else
        {
            FilePathSelect(-1);
            Debug.Log("話題が解除されました");
            NowBottonPushed = -1;
        }

    }
    public void WadaiButton1()
    {
        if (NowBottonPushed != 1)
        {
            FilePathSelect(1);
            Debug.Log("話題1が押されました");
            NowBottonPushed = 1;
        }
        else
        {
            FilePathSelect(-1);
            Debug.Log("話題が解除されました");
            NowBottonPushed = -1;
        }
    }
    public void WadaiButton2()
    {
        if (NowBottonPushed != 2)
        {
            FilePathSelect(2);
            Debug.Log("話題2が押されました");
            NowBottonPushed = 2;
        }
        else
        {
            FilePathSelect(-1);
            Debug.Log("話題が解除されました");
            NowBottonPushed = -1;
        }
    }
    public void WadaiButton3()
    {
        if (NowBottonPushed != 3)
        {
            FilePathSelect(3);
            Debug.Log("話題3が押されました");
            NowBottonPushed = 3;
        }
        else
        {
            FilePathSelect(-1);
            Debug.Log("話題が解除されました");
            NowBottonPushed = -1;
        }
    }
    public void WadaiButton4()
    {
        if (NowBottonPushed != 4)
        {
            FilePathSelect(4);
            Debug.Log("話題4が押されました");
            NowBottonPushed = 4;
        }
        else
        {
            FilePathSelect(-1);
            Debug.Log("話題が解除されました");
            NowBottonPushed = -1;
        }
    }
    public void WadaiButton5()
    {
        if (NowBottonPushed != 5)
        {
            FilePathSelect(5);
            Debug.Log("話題5が押されました");
            NowBottonPushed = 5;
        }
        else
        {
            FilePathSelect(-1);
            Debug.Log("話題が解除されました");
            NowBottonPushed = -1;
        }
    }
    public void WadaiButton6()
    {
        if (NowBottonPushed != 6)
        {
            FilePathSelect(6);
            Debug.Log("話題6が押されました");
            NowBottonPushed = 6;
        }
        else
        {
            FilePathSelect(-1);
            Debug.Log("話題が解除されました");
            NowBottonPushed = -1;
        }
    }
    public void WadaiButton7()
    {
        if (NowBottonPushed != 7)
        {
            FilePathSelect(7);
            Debug.Log("話題7が押されました");
            NowBottonPushed = 7;
        }
        else
        {
            FilePathSelect(-1);
            Debug.Log("話題が解除されました");
            NowBottonPushed = -1;
        }
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
