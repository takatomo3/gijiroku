using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MonobitEngine;
using System.IO;
using System.ComponentModel;
using System;
using System.Linq;
using UnityEngine.UI;

public class MargeTxt : MonobitEngine.MonoBehaviour
{

    string MargefilePath;
    string InputPath;
    string LogDataPath;
    int player;
    [SerializeField]
    GameObject PopUp;

    // Start is called before the first frame update
    void Start()
    {
        Clear();
        //Debug.Log("start");
    }

    void Clear()
    {
        PopUp.GetComponent<Text>().text = "";
    }


    [MunRPC, MenuItem("Example/Copy Something")]
    public void Share(string data, string path, string file)
    {
        //FileUtil.CopyFileOrDirectory(folder, "Assets/MargeFolder");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        FileStream stream = File.Create(file);
        stream.Close();
        File.WriteAllText(file, data);
    }



    //フォルダのコピー生成
    [MunRPC, MenuItem("Example/Copy Something")]
    public void CopySomething(string timeStamp)
    {
        string path = "Assets/MargeFolder/" + timeStamp;
        FileUtil.CopyFileOrDirectory("Assets/MargeCSVLogFiles", path/*"Assets/MargeFolder/" + timeStamp*/);
        Debug.Log("コピーしました");

        string[] files = System.IO.Directory.GetFiles(path, "*.csv", System.IO.SearchOption.AllDirectories);
        //string path = "Assets/MargeFolder/" + timeStamp;
        //string data = File.ReadAllText(path);

        foreach (var file in files)
        {
            Debug.Log(file);
            string data = File.ReadAllText(file);
            monobitView.RPC("Share", MonobitTargets.Others, data, path, file);
        }

        //IEnumerable<string> subFolders = System.IO.Directory.EnumerateDirectories("Assets", "MargeFolder", System.IO.SearchOption.AllDirectories);
        //monobitView.RPC("Share", MonobitTargets.Others, data, path);
    }

    //マージファイルに書き込み
    [MunRPC]
    public void RecvChat(string list)
    {
        PopUp.GetComponent<Text>().text += Environment.NewLine;
        PopUp.GetComponent<Text>().text += list;

    }

    //一文ずつ送信
    [MunRPC]
    public void Send()
    {
        Clear();
        //int cnt;
        //player = MonobitEngine.MonobitNetwork.room.playerCount;
        //for (int i = 0; i < 9; i++)
        //{
            LogDataName(0);
            string LogPath = Application.dataPath + LogDataPath;
            //書き込み先ファイルの指定
            var fs = new FileStream(LogPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            //　ストリームで読み込みと書き込み
            using (StreamReader streamReader = new StreamReader(fs))
            {
                List<string> lists = new List<string>();

                while (!streamReader.EndOfStream)
                {
                    lists.AddRange(streamReader.ReadLine().Split('\n'));
                }
            //cnt = lists.Count();

                foreach (var list in lists)
                {
                    //cnt--;
                    monobitView.RPC("RecvChat", MonobitTargets.All, list);
                }
            }



        //}
    }

    DateTime time;
    string timeStamp;

    //outputボタンをクリックした時に送信
    public void ClickFlag()
    {
        monobitView.RPC("Send", MonobitTargets.All);
    }

    //ソート関数
    //[MunRPC]
    public void MargeSort()
    {
        for (int i = 0; i < 9; i++)
        {
            MargePathName(i);
            string SortFilePath = Application.dataPath + MargefilePath;
            List<string> lists = new List<string>();
            using (FileStream fileStream = File.Open(SortFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    while (!streamReader.EndOfStream)
                    {
                        lists.AddRange(streamReader.ReadLine().Split('\n'));
                    }

                    lists.Sort();

                    fileStream.SetLength(0);

                    foreach (var list in lists)
                    {
                        streamWriter.Write(list);
                        streamWriter.WriteLine();
                    }
                }
            }
        }
        time = DateTime.Now;
        timeStamp = time.ToString("yyyy_MMdd_HH_mm_ss");
        Debug.Log(timeStamp);
        monobitView.RPC("CopySomething", MonobitTargets.Host, timeStamp);

        //ソート前のリスト
        //Debug.Log("ソート前のリスト" + string.Join("", sortlists));
        //ShowListContentsInTheDebugLog(sortlists);
        //sortlists.Sort();
        //ソート後のリスト
        //Debug.Log("ソート後のリスト" + string.Join("", sortlists));
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
            case 8:
                MargefilePath = @"/MargeCSVLogFiles/MargeCSVLogFile.csv";
                break;
            default:
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
            case 8:
                InputPath = @"/CSVLogFiles/CSVLogFile.csv";
                break;
            default:
                break;
        }
    }

    void LogDataName(int number)
    {
        switch (number)
        {
            case 0:
                LogDataPath = @"/LogDatas/LogData0.txt";
                break;
            case 1:
                LogDataPath = @"/LogDatas/LogData1.txt";
                break;
            case 2:
                LogDataPath = @"/LogDatas/LogData2.txt";
                break;
            case 3:
                LogDataPath = @"/LogDatas/LogData3.txt";
                break;
            case 4:
                LogDataPath = @"/LogDatas/LogData4.txt";
                break;
            case 5:
                LogDataPath = @"/LogDatas/LogData5.txt";
                break;
            case 6:
                LogDataPath = @"/LogDatas/LogData6.txt";
                break;
            case 7:
                LogDataPath = @"/LogDatas/LogData7.txt";
                break;
            default:
                LogDataPath = @"/LogDatas/LogData.txt";
                break;
        }
    }
}
