using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
using UnityEngine.SceneManagement;

public class MainSecneMUNScript : MonobitEngine.MonoBehaviour
{
    /** ルーム名. */
    private string roomName = "";

    /** チャット発言文. */
    private string chatWord = "";

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


        //Debug.Log("ルームに入室しました");
        // ルーム内のプレイヤー一覧の表示
        /*
        foreach (RoomData room in MonobitNetwork.GetRoomData())
        {
            roomName = room.name;
        }
        */
        
        GUILayout.BeginHorizontal();
        roomName = MonobitEngine.MonobitNetwork.room.name;
        GUILayout.Label("roomName : " + roomName + "\n");
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
                /*
                // チャット発言文の入力
                GUILayout.BeginHorizontal();
                GUILayout.Label("Message : ");
                chatWord = GUILayout.TextField(chatWord, GUILayout.Width(400));
                GUILayout.EndHorizontal();

                // チャット発言文を送信する
                if (GUILayout.Button("Send", GUILayout.Width(100)))
                {
                    monobitView.RPC("RecvChat",
                                    MonobitTargets.All,
                                    MonobitNetwork.playerName,
                                    chatWord);
                    chatWord = "";
                }

                // チャットログを表示する
                string msg = "";
                for (int i = 0; i < 10; ++i)
                {
                    msg += ((i < chatLog.Count) ? chatLog[i] : "") + "\r\n";
                }
                GUILayout.TextArea(msg);
                */
            
            




    }
}
