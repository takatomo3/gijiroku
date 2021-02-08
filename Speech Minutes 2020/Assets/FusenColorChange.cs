using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FusenColorChange : MonoBehaviour
{
    [SerializeField]
    GameObject FusenPanel;
    Dropdown dropdown;

    // Start is called before the first frame update
    void Start()
    {
        dropdown = GetComponent<Dropdown>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dropdown.value == 0)
        {
            FusenPanel.GetComponent<Image>().color = Color.magenta;
        }

        if (dropdown.value == 1)
        {
            FusenPanel.GetComponent<Image>().color = Color.yellow;
        }

        if (dropdown.value == 2)
        {
            FusenPanel.GetComponent<Image>().color = Color.green;
        }

    }
}
