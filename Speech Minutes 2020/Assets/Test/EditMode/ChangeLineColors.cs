using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLineColors : MonoBehaviour
{
    GameObject whiteboard;
    PaintController script;
    Dropdown dropdown;

    // Start is called before the first frame update
    void Start()
    {
        whiteboard = GameObject.Find("WhiteBoard");
        script = whiteboard.GetComponent<PaintController>();
        dropdown = GetComponent<Dropdown>();
    }

    // Update is called once per frame
    void Update()
    {
        if(dropdown.value == 0){
            script.lineColor = Color.black;
        }

        if(dropdown.value == 1){
            script.lineColor = Color.red;
        }

        if(dropdown.value == 2){
            script.lineColor = Color.blue;
        }

        if(dropdown.value == 3){
            script.lineColor = Color.green;
        }

        if(dropdown.value == 4){
            script.lineColor = Color.yellow;
        }

        if(dropdown.value == 5){
            script.lineColor = Color.magenta;
        }
    }
}
