using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inversion : MonoBehaviour
{
    GameObject whiteboard;
    GameObject comentext;
    PixAccess script;
    TextControl texscript;
    Dropdown dropdown;
    int inversionFlag = 0;

    public GameObject canvas;//キャンバス
    public GameObject gametext;
    // Start is called before the first frame update
    void Start()
    {
        whiteboard = GameObject.Find("Plane");
        script = whiteboard.GetComponent<PixAccess>();
      //  comentext = (GameObject)Resources.Load("TextOutput");
       // texscript = comentext.GetComponent<TextControl>();
        dropdown = GetComponent<Dropdown>();
        
    }
	/// <summary>
    /// 白黒反転
    /// </summary>
    public void inversion()
    {
        script.Start();
       if(inversionFlag==0)
        {
            /*script.bgColor = Color.black;
           // texscript.texcolor = Color.white;*/
            inversionFlag =1;
        Debug.Log("黒");
        }
    else
       if(inversionFlag==1)
        {
       /* script.bgColor = Color.white;
      //  texscript.texcolor = Color.black;*/
        inversionFlag=0;
        Debug.Log("白");
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}