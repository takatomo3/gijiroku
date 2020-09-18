using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class TextControl : MonoBehaviour, IDragHandler
{
    public void Update()
    {
        
    }

    public RectTransform m_rectTransform = null;

    private void Reset()
    {
        m_rectTransform = GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData e)
    {
        m_rectTransform.position += new Vector3(e.delta.x, e.delta.y, 0f);
    }

    //テキストボックスの削除(バックスペースで削除)
    public void Destroy()
    {
        if (Input.GetKey(KeyCode.Backspace)) {
            Destroy(this.gameObject);
            Debug.Log("ok");
        }
    }
}
