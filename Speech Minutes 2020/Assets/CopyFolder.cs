using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MonobitEngine;
using System.IO;
using System.ComponentModel;
using System;

public class CopyFolder : MonobitEngine.MonoBehaviour
{
    DateTime time;
    string timeStamp;

    public void CopyFlag()
    {
        time = DateTime.Now;
        timeStamp = time.ToString("yyyy_MMdd_HH_mm_ss");
        monobitView.RPC("CopySomething", MonobitTargets.Host, timeStamp);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [MunRPC, MenuItem("Example/Copy Something")]
    public void CopySomething(string timeStamp)
    {
        FileUtil.CopyFileOrDirectory("Assets/MargeCSVLogFiles", "Assets/MargeFolder/" + timeStamp);
        Debug.Log("コピーしました");

        //IEnumerable<string> subFolders = System.IO.Directory.EnumerateDirectories("Assets", "MargeFolder", System.IO.SearchOption.AllDirectories);
        //monobitView.RPC("Share", MonobitTargets.Others, subFolders);
    }

    
}
