using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
using UnityEngine.SceneManagement;
using MonobitEngine.VoiceChat;

public class MainSecneMUNScript : MonobitEngine.MonoBehaviour
{
    /** ルーム名. */
    private string roomName = "";


    /** チャット発言ログ. */
    List<string> chatLog = new List<string>();

    /**
    * RPC 受信関数.
    */
    [MunRPC]
    void RecvChat(string senderName, string senderWord)
    {
        chatLog.Add(senderName + " : " + senderWord);
        if (chatLog.Count > 10)
        {
            chatLog.RemoveAt(0);
        }
    }

    

    

    private void OnGUI()
    {
        //MUNサーバに接続している場合
        if (MonobitNetwork.isConnect)
        {
            GUILayout.BeginHorizontal();
            //Debug.Log(MonobitEngine.MonobitNetwork.room.name);
            roomName = MonobitEngine.MonobitNetwork.room.name;
            GUILayout.Label("roomName : " + roomName);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("PlayerList : ");

            //Debug.Log("PlayerList:");
            foreach (MonobitPlayer player in MonobitNetwork.playerList)
            {
                GUILayout.Label(player.name + " ");
                //Debug.Log(player.name + " ");
            }
            GUILayout.EndHorizontal();

            // ルームからの退室
            if (GUILayout.Button("Leave Room", GUILayout.Width(150)))
            {
                MonobitNetwork.LeaveRoom();
                //Debug.Log("ルームから退出しました");
                /********
               ここでメインのシーンに遷移する
               *********/
                SceneManager.LoadScene("StartScene");
            }

        }
    }
}
