using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextControl : MonoBehaviour
{
    public void Update()
    {
        
    }

    public void Destroy()
    {
        if (Input.GetKey(KeyCode.Backspace)) {
            Destroy(this.gameObject);
            Debug.Log("ok");
        }
    }
}
