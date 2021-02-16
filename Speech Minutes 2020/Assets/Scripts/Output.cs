
using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using MonobitEngine;
public class Output : MonobitEngine.MonoBehaviour
{
	string InputPath;
    string OutputPath;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [MunRPC]
    public void OnClick()
    {
        for (int i = 0; i < 9; i++)
        {
            InputPathName(i);                                                   //CSVファイルの書き込み元ファイルを選択します
            string CSVFilePath = Application.dataPath + InputPath;              //書き込み元ファイルの指定
            OutputPathName(i);                                                  //CSVファイルの書き込み先ファイルを選択します
            string CSVWriteFilePath = Application.dataPath + OutputPath;        //書き込み先ファイルの指定

            var fs = new FileStream(CSVFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            //　ストリームで読み込みと書き込み
            using (StreamReader streamReader = new StreamReader(fs))
            using (StreamWriter streamWriter = new StreamWriter(CSVWriteFilePath))
            {

                List<string> lists = new List<string>();

                while (!streamReader.EndOfStream)
                {
                    lists.AddRange(streamReader.ReadLine().Split(','));
                }

                int count = 0;
                foreach (var list in lists)
                {
                    streamWriter.Write(list.ToString() + ',');
                    count++;
                    if (count % 4 == 0)
                    {
                        streamWriter.WriteLine();
                    }
                }

                foreach (var list in lists)
                {
                    Debug.Log(list);
                }
            }
        }

	}

    public void OutputFlag()
    {
        monobitView.RPC("OnClick", MonobitTargets.All);
    }

    //CSVファイルの書き込み元ファイルを選択します
    void InputPathName(int number)
    {
        switch (number)
        {
            case 0:
                InputPath = @"/LogDatas/LogData0.txt";
                break;
            case 1:
                InputPath = @"/LogDatas/LogData1.txt";
                break;
            case 2:
                InputPath = @"/LogDatas/LogData2.txt";
                break;
            case 3:
                InputPath = @"/LogDatas/LogData3.txt";
                break;
            case 4:
                InputPath = @"/LogDatas/LogData4.txt";
                break;
            case 5:
                InputPath = @"/LogDatas/LogData5.txt";
                break;
            case 6:
                InputPath = @"/LogDatas/LogData6.txt";
                break;
            case 7:
                InputPath = @"/LogDatas/LogData7.txt";
                break;
            default:
                InputPath = @"/LogDatas/LogData.txt";
                break;
        }
    }

    //CSVファイルの書き込み先ファイルを選択します
    void OutputPathName(int number)
    {
        switch (number)
        {
            case 0:
                OutputPath = @"/CSVLogFiles/CSVLogFile0.csv";
                break;
            case 1:
                OutputPath = @"/CSVLogFiles/CSVLogFile1.csv";
                break;
            case 2:
                OutputPath = @"/CSVLogFiles/CSVLogFile2.csv";
                break;
            case 3:
                OutputPath = @"/CSVLogFiles/CSVLogFile3.csv";
                break;
            case 4:
                OutputPath = @"/CSVLogFiles/CSVLogFile4.csv";
                break;
            case 5:
                OutputPath = @"/CSVLogFiles/CSVLogFile5.csv";
                break;
            case 6:
                OutputPath = @"/CSVLogFiles/CSVLogFile6.csv";
                break;
            case 7:
                OutputPath = @"/CSVLogFiles/CSVLogFile7.csv";
                break;
            default:
                OutputPath = @"/CSVLogFiles/CSVLogFile.csv";
                break;
        }
    }
}
