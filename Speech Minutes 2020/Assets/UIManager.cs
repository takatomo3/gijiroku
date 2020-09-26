using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject WadaiPanel;
    [SerializeField] GameObject WhiteBoardPanel;
    // Start is called before the first frame update

    bool EnableWadaiPanel = true;
    bool EnableWhiteBoardPanel = true;

    void Start()
    {
        EnableWadaiPanel = true;
        EnableWhiteBoardPanel = true;
    }

    public void OnclickWadaiButton()
    {
        WadaiPanel.SetActive(EnableWadaiPanel);
        EnableWadaiPanel = !EnableWadaiPanel;
    }

    public void OnclickWhiteBoardButton()
    {
        WhiteBoardPanel.SetActive(EnableWhiteBoardPanel);
        EnableWhiteBoardPanel = !EnableWhiteBoardPanel;

    }



}
