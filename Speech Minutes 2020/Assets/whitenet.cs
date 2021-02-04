using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class whitenet : MonobitEngine.MonoBehaviour
{
    private string roomName = "";

    // Start is called before the first frame update
    void Start()
    {
        // デフォルトロビーへの自動入室を許可する
        MonobitNetwork.autoJoinLobby = true;

        // MUNサーバに接続する
        MonobitNetwork.ConnectServer("Bearpocalypse_v1.0");
    }

    // Update is called once per frame
    void OnGUI()
    {
        // デフォルトのボタンと被らないように、段下げを行なう。
        GUILayout.Space(24);

        // MUNサーバに接続している場合
        if (MonobitNetwork.isConnect)
        {
            // ルームに入室していない場合
            if (!MonobitNetwork.inRoom)
            {
                GUILayout.BeginHorizontal();

                // ルーム名の入力
                GUILayout.Label("RoomName : ");
                roomName = GUILayout.TextField(roomName, GUILayout.Width(200));

                // ボタン入力でルーム作成
                if (GUILayout.Button("Create Room", GUILayout.Width(150)))
                {
                    MonobitNetwork.CreateRoom(roomName);
                }

                GUILayout.EndHorizontal();

            }
        }
    }
}
