using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject WadaiPanel;
    [SerializeField] GameObject WhiteBoardPanel;
    [SerializeField] GameObject SendTextPanel;
    // Start is called before the first frame update

    bool EnableWadaiPanel = true;
    bool EnableWhiteBoardPanel = true;
    bool EnableSendTextPanel = true;

    void Start()
    {
        EnableWadaiPanel = true;
        EnableWhiteBoardPanel = true;
        EnableSendTextPanel = true;
    }

    public void OnclickWadaiButton()
    {
        WhiteBoardPanel.SetActive(false);
        SendTextPanel.SetActive(false);
        EnableWhiteBoardPanel = true;
        EnableSendTextPanel = true;

        WadaiPanel.SetActive(EnableWadaiPanel);
        EnableWadaiPanel = !EnableWadaiPanel;
    }

    public void OnclickWhiteBoardButton()
    {
        WadaiPanel.SetActive(false);
        SendTextPanel.SetActive(false);
        EnableWadaiPanel = true;
        EnableSendTextPanel = true;

        WhiteBoardPanel.SetActive(EnableWhiteBoardPanel);
        EnableWhiteBoardPanel = !EnableWhiteBoardPanel;
    }

    public void OnclickSendTextButton()
    {
        WadaiPanel.SetActive(false);
        WhiteBoardPanel.SetActive(false);
        EnableWadaiPanel = true;
        EnableWhiteBoardPanel = true;

        SendTextPanel.SetActive(EnableSendTextPanel);
        EnableSendTextPanel = !EnableSendTextPanel;
    }



}
