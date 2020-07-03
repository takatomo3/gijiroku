
using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System;
public class Output : MonoBehaviour
{
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
     
		string CSVFilePath = Application.dataPath + @"\Scripts\File\hunanichi_hu.csv";
		string CSVWriteFilePath = Application.dataPath + @"\Scripts\File\CSVWriteFile.csv";

		//　ストリームで読み込みと書き込み
		using (StreamReader streamReader = new StreamReader(CSVFilePath))
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
				if (count % 3 == 0)
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
