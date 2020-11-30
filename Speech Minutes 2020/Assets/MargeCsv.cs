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

    string filePath;

    // Start is called before the first frame update
    void Start()
    {
        /*
        for (int i = 0; i < 8; i++)
        {
            //filePathのパス指定
            FilePathSelect(i);
            File.CreateText(filePath);
            if (i == 7)
            {
                FilePathSelect(-1); break;
            }
        }
        */
        filePath = Application.dataPath + @"/MargeCSVLogFile/MargeCSVLogFile0.csv";
        File.CreateText(filePath);
    }

    [MunRPC]
    public void RecvChat(string list)
    {
        string CSVWriteFilePath = Application.dataPath + @"/MargeCSVLogFile/MargeCSVLogFile0.csv";
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
        string CSVFilePath = Application.dataPath + @"/CSVLogFiles/CSVLogFile0.csv";
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
            foreach(var list in lists)
            {
                monobitView.RPC("RecvChat", MonobitTargets.Host, list);
            }   
        }
    } 
}
