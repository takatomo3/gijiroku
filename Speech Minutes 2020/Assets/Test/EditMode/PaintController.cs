using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class PaintController : MonoBehaviour {

    /// <summary>
    /// 描く線のコンポーネントリスト
    /// </summary>
    private List<LineRenderer> lineRendererList;

    /// <summary>
    /// 描く線のマテリアル
    /// </summary>
    public Material lineMaterial;

    /// <summary>
    /// 描く線の色
    /// </summary>
    public Color lineColor;

    /// <summary>
    /// 描く線の太さ
    /// </summary>
    [Range(0,10)] public float lineWidth;

    public bool mode = false;   //PenMode用フラグ

    public GameObject PenMode;

    //public bool onetime = true;
    //public bool add = false;

    //private Vector3 touchPos;

    //public float lineLength = 0.01f;

    void Start () {
        PenMode = GameObject.Find("PenMode");
    }

    void Awake () {
        lineRendererList = new List<LineRenderer>();
    }

    void Update () {

        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
        //Debug.Log(hit2d.collider);

        // ボタンが押された時に線オブジェクトの追加を行う
        if (Input.GetMouseButtonDown(0) && mode) {
            this.AddLineObject();
            //touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //touchPos.z=0;
        }

        // ボタンが押されている時、LineRendererに位置データの設定を指定していく
        if (Input.GetMouseButton(0) && mode) {
            this.AddPositionDataToLineRendererList();

            /*if(onetime){
                Vector3 startPos = touchPos;
                Vector3 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                endPos.z=0;
                if((endPos-startPos).magnitude > lineLength){
                    this.AddLineObject();
                    onetime = false;
                    add = true;
                }
            }

            if(add){
                this.AddPositionDataToLineRendererList();
            }*/
        }

        /*if(Input.GetMouseButtonUp(0)){
            onetime = true;
            add = false;
        }*/
    }

    /// <summary>
    /// 線オブジェクトの追加を行うメソッド
    /// </summary>
    private void AddLineObject () {

        // 追加するオブジェクトをインスタンス
        GameObject lineObject = new GameObject();
        lineObject.tag = "line";

        // オブジェクトにLineRendererを取り付ける
        lineObject.AddComponent<LineRenderer>();
        lineObject.AddComponent<MeshCollider>();

        // 描く線のコンポーネントリストに追加する
        lineRendererList.Add(lineObject.GetComponent<LineRenderer>());

        // 線と線をつなぐ点の数を0に初期化
        lineRendererList.Last().positionCount = 0;

        // マテリアルを初期化
        lineRendererList.Last().material = this.lineMaterial;

        // 線の色を初期化
        lineRendererList.Last().material.color = this.lineColor;

        // 線の太さを初期化
        lineRendererList.Last().startWidth = this.lineWidth;
        lineRendererList.Last().endWidth   = this.lineWidth;
    }

    /// <summary>
    /// 描く線のコンポーネントリストに位置情報を登録していく
    /// </summary>
    private void AddPositionDataToLineRendererList () {

        // 座標の変換を行いマウス位置を取得
        Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 1.0f);
        var mousePosition = Camera.main.ScreenToWorldPoint(screenPosition);

        // 線と線をつなぐ点の数を更新
        lineRendererList.Last().positionCount += 1;

        // 描く線のコンポーネントリストを更新
        lineRendererList.Last().SetPosition(lineRendererList.Last().positionCount - 1, mousePosition);
    }

    //一括削除用
    public void Clear(){
        var clones = GameObject.FindGameObjectsWithTag ("line");
        foreach (var clone in clones)
        {
            Destroy(clone);
        }
    }

    //PenMode切り替え用
    public void Mode(){
        if(mode){
            mode = false;
            Debug.Log("PenMode:" + mode);
            PenMode.GetComponentInChildren<Text> ().text = "PenMode:false";
        }else{
            mode = true;
            Debug.Log("PenMode:" + mode);
            PenMode.GetComponentInChildren<Text> ().text = "PenMode:true";
        }
    }
}