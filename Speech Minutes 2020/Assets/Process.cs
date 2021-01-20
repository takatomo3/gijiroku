using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Process : MonoBehaviour
{
    public static void Main()
    {
        Process proc = new Process();             // （1）
        proc.StartInfo.FileName = "MargeCsv.cs";   // （2）
        proc.Start();                             // （3）
        proc.WaitForExit();                    // （4）
        Console.WriteLine("スクリプト、終了！");
    }
}

// Start is called before the first frame update
void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
