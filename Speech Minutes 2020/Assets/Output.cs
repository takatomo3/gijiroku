
using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System;
public class Output : MonoBehaviour
{
	public voiceSample vS;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClick()
    {

		string CSVFilePath = Application.dataPath + @"\LogData.txt";
		string CSVWriteFilePath = Application.dataPath + @"\CSVLogFile.csv";

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
	}
}
