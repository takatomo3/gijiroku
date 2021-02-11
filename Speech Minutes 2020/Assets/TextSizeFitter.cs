using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextSizeFitter : MonoBehaviour
{
    Text text;
    [SerializeField]
    GameObject Panel;
    float Width = 56f;
    float Height = 15f;
    float Bairitsu = 2.0f;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text = this.GetComponent<Text>();

        //取得したTextをピッタリ収まるようにサイズ変更(Heightが長い状態)
        text.rectTransform.sizeDelta = new Vector2(text.preferredWidth, text.preferredHeight);

        //再度、ピッタリ収まるようにサイズ変更(Heightもピッタリ合うように)
        text.rectTransform.sizeDelta = new Vector2(text.preferredWidth, text.preferredHeight);

        Panel.GetComponent<RectTransform>().sizeDelta = text.rectTransform.sizeDelta * Bairitsu;


    }
}
