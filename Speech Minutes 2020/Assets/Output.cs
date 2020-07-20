using UnityEngine;using System.Collections;using System.IO;using System.Collections.Generic;using System.ComponentModel;using System;public class Output : MonoBehaviour{	string InputPath;
    string OutputPath;
    // Start is called before the first frame update
    void Start()    {            }    // Update is called once per frame    void Update()    {            }    public void OnClick()    {
        for (int i = 0; i < 5; i++)
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
                    if (count % 2 == 0)
                    {
                        streamWriter.WriteLine();
                    }
                }

                foreach (var list in lists)
                {
                    Debug.Log(list);
                }
            }
        }	}

    //CSVファイルの書き込み元ファイルを選択します
    void InputPathName(int number)
    {
        switch (number)
        {
            case 0:
                InputPath = @"\LogData0.txt";
                break;
            case 1:
                InputPath = @"\LogData1.txt";
                break;
            case 2:
                InputPath = @"\LogData2.txt";
                break;
            case 3:
                InputPath = @"\LogData3.txt";
                break;
            default:
                InputPath = @"\LogData.txt";
                break;
        }
    }

    //CSVファイルの書き込み先ファイルを選択します
    void OutputPathName(int number)
    {
        switch (number)
        {
            case 0:
                OutputPath = @"\CSVLogFile0.csv";
                break;
            case 1:
                OutputPath = @"\CSVLogFile1.csv";
                break;
            case 2:
                OutputPath = @"\CSVLogFile2.csv";
                break;
            case 3:
                OutputPath = @"\CSVLogFile3.csv";
                break;
            default:
                OutputPath = @"\CSVLogFile.csv";
                break;
        }
    }}