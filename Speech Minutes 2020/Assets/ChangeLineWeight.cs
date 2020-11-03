using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLineWeight : MonoBehaviour
{
    GameObject whiteboard;
    Painter script;
    Dropdown dropdown;
    // Start is called before the first frame update
    void Start()
    {
        whiteboard = GameObject.Find("drowingboad"); //Unityちゃんをオブジェクトの名前から取得して変数に格納する
        script = whiteboard.GetComponent<Painter>();
        dropdown = GetComponent<Dropdown>();
    }

    // Update is called once per frame
    void Update()
    {
        if(dropdown.value == 0){
            script.lineWidth = 0.02f;
        }

        if(dropdown.value == 1){
            script.lineWidth = 0.04f;
        }

        if(dropdown.value == 2){
            script.lineWidth = 0.08f;
        }
    }
}
