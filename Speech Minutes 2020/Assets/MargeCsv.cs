using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MonobitEngine;
using System.IO;
using System.ComponentModel;
using System;

public class MargeCsv : MonobitEngine.MonoBehaviour
{

    string MargefilePath;
    string InputPath;

    // Start is called before the first frame update
    void Start()
    {
        
        for (int i = 0; i < 8; i++)
        {
            //filePathのパス指定
            MargePathName(i);
            File.CreateText(Application.dataPath + MargefilePath);
            if (i == 7)
            {
                MargePathName(-1); break;
            }
        }
        MargefilePath = Application.dataPath + @"/MargeCSVLogFiles/MargeCSVLogFile.csv";
        File.CreateText(MargefilePath);
        
    }

    [MunRPC]
    public void RecvChat(string list, int i)
    {
        MargePathName(i);
        string CSVWriteFilePath = Application.dataPath + MargefilePath;
        Debug.Log(MargefilePath);
        using (StreamWriter streamWriter = new StreamWriter(CSVWriteFilePath, true))
        {
            streamWriter.Write(list);
            streamWriter.WriteLine();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void marge()
    {
        //FileUtil.CopyFileOrDirectory("CSVLogFiles/CSVLogFile.csv", "MargeCSV/MargeCSVLogFile.csv");
        //AssetDatabase.CopyAsset("Assets/CSVLogFiles", "Assets/MargeCSV");
    }

    public void Send()
    {
        for (int i = 0; i < 9; i++)
        {
            MargePathName(i);
            string ClearPath = Application.dataPath + MargefilePath;
            using (var fileStream = new FileStream(ClearPath, FileMode.Open))
            {
                // ストリームの長さを0に設定します。
                // 結果としてファイルのサイズが0になります。
                fileStream.SetLength(0);
                
            }
        }

        for (int i = 0; i < 9; i++)
        { 
            InputPathName(i);
            string CSVFilePath = Application.dataPath + InputPath;
            //書き込み先ファイルの指定


            var fs = new FileStream(CSVFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            //　ストリームで読み込みと書き込み
            using (StreamReader streamReader = new StreamReader(fs))

            {
                List<string> lists = new List<string>();

                while (!streamReader.EndOfStream)
                {
                    lists.AddRange(streamReader.ReadLine().Split('\n'));
                }
                foreach (var list in lists)
                {
                    monobitView.RPC("RecvChat", MonobitTargets.Host, list, i);
                }
            }
        }
    }

    void MargePathName(int number)
    {
        switch (number)
        {
            case 0:
                MargefilePath = @"/MargeCSVLogFiles/MargeCSVLogFile0.csv";
                break;
            case 1:
                MargefilePath = @"/MargeCSVLogFiles/MargeCSVLogFile1.csv";
                break;
            case 2:
                MargefilePath = @"/MargeCSVLogFiles/MargeCSVLogFile2.csv";
                break;
            case 3:
                MargefilePath = @"/MargeCSVLogFiles/MargeCSVLogFile3.csv";
                break;
            case 4:
                MargefilePath = @"/MargeCSVLogFiles/MargeCSVLogFile4.csv";
                break;
            case 5:
                MargefilePath = @"/MargeCSVLogFiles/MargeCSVLogFile5.csv";
                break;
            case 6:
                MargefilePath = @"/MargeCSVLogFiles/MargeCSVLogFile6.csv";
                break;
            case 7:
                MargefilePath = @"/MargeCSVLogFiles/MargeCSVLogFile7.csv";
                break;
            default:
                MargefilePath = @"/MargeCSVLogFiles/MargeCSVLogFile.csv";
                break;
        }
    }

    void InputPathName(int number)
    {
        switch (number)
        {
            case 0:
                InputPath = @"/CSVLogFiles/CSVLogFile0.csv";
                break;
            case 1:
                InputPath = @"/CSVLogFiles/CSVLogFile1.csv";
                break;
            case 2:
                InputPath = @"/CSVLogFiles/CSVLogFile2.csv";
                break;
            case 3:
                InputPath = @"/CSVLogFiles/CSVLogFile3.csv";
                break;
            case 4:
                InputPath = @"/CSVLogFiles/CSVLogFile4.csv";
                break;
            case 5:
                InputPath = @"/CSVLogFiles/CSVLogFile5.csv";
                break;
            case 6:
                InputPath = @"/CSVLogFiles/CSVLogFile6.csv";
                break;
            case 7:
                InputPath = @"/CSVLogFiles/CSVLogFile7.csv";
                break;
            default:
                InputPath = @"/CSVLogFiles/CSVLogFile.csv";
                break;
        }
    }

}
