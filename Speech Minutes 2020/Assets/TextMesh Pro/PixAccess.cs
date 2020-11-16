using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class PixAccess : MonoBehaviour
{
	Texture2D drawTexture;
	Color[] buffer;
	/// <summary>
    /// ボタンと連動
    /// </summary>
	public GameObject PenMode;
	public bool mode = false;
	public Text Buttontext;
	public Material lineMaterial;
	public Color lineColor;
	[Range(0, 64)] public float lineWidth;
	/// <summary>
    /// 白黒反転
    /// </summary>
	int inversionFlag = 0;

	public void Start()
	{
		Texture2D mainTexture = (Texture2D)GetComponent<Renderer>().material.mainTexture;
		Color[] pixels = mainTexture.GetPixels();

		buffer = new Color[pixels.Length];
		pixels.CopyTo(buffer, 0);

		drawTexture = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);
		drawTexture.filterMode = FilterMode.Point;

		PenMode = GameObject.Find("PenMode");
	}

	//PenMode切り替え用
	public void Mode()
	{
		if (mode)
		{
			mode = false;
			Debug.Log("PenMode:" + mode);
			Buttontext.text = "PenMode:false";
		}
		else
		{
			mode = true;
			Debug.Log("PenMode:" + mode);
			Buttontext.text = "PenMode:true";
		}
	}
	/// <summary>
    /// 初期化
    /// </summary>
	public void Clear()
	{
		Start();
	}
	/// <summary>
    /// 太さ変更
    /// </summary>
    /// <param name="p"></param>
	public void Draw(Vector2 p)
	{
		for (int x = 0; x < 256; x++)
		{
			for (int y = 0; y < 256; y++)
			{
				if ((p - new Vector2(x, y)).magnitude < lineWidth)
				{
					buffer.SetValue(lineColor, x + 256 * y);
				}
			}
		}
	}

	public void inversion()
	{
		Texture2D mainTexture = (Texture2D)GetComponent<Renderer>().material.mainTexture;
		Color[] pixels = mainTexture.GetPixels();

		buffer = new Color[pixels.Length];
		pixels.CopyTo(buffer, 0);
		if (inversionFlag == 0)
		{
            /*script.bgColor = Color.black;
           // texscript.texcolor = Color.white;*/
            for (int x = 0; x < mainTexture.width; x++)
			{
				for (int y = 0; y < mainTexture.height; y++)
				{
					if (y < mainTexture.height)
					{
						buffer.SetValue(Color.black, x + 256 * y);
					}
				}
			}
			inversionFlag = 1;
			Debug.Log("黒");
		}
		else
		if (inversionFlag == 1)
		{
			/* script.bgColor = Color.white;
		   //  texscript.texcolor = Color.black;*/
			for (int x = 0; x < mainTexture.width; x++)
			{
				for (int y = 0; y < mainTexture.height; y++)
				{
					if (y < mainTexture.height)
					{
						buffer.SetValue(Color.white, x + 256 * y);
					}
				}
			}
			inversionFlag = 0;
			Debug.Log("白");
		}

		drawTexture = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);
		drawTexture.filterMode = FilterMode.Point;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100.0f))
		{
			Draw(hit.textureCoord * 256);
		}

		drawTexture.SetPixels(buffer);
		drawTexture.Apply();
		GetComponent<Renderer>().material.mainTexture = drawTexture;
	}
	void Update()
    {
		
		if (mode)
		{
			if (Input.GetMouseButton(0))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 100.0f))
				{
					Draw(hit.textureCoord * 256);
				}

				drawTexture.SetPixels(buffer);
				drawTexture.Apply();
				GetComponent<Renderer>().material.mainTexture = drawTexture;
			}
		}
	}
}