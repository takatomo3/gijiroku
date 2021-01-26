/**
* (C) Copyright IBM Corp. 2015, 2020.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*
*/
#pragma warning disable 0649

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//using IBM.Watson.SpeechToText.V1;
//using IBM.Cloud.SDK;
//using IBM.Cloud.SDK.Authentication;
//using IBM.Cloud.SDK.Authentication.Iam;
//using IBM.Cloud.SDK.Utilities;
//using IBM.Cloud.SDK.DataTypes;

using System.IO;
using MonobitEngine;
using MonobitEngine.VoiceChat;
//using MonobitEngineBase;
using System;
using UnityEngine.SceneManagement;
namespace FrostweepGames.Plugins.GoogleCloud.SpeechRecognition
{
    public class ExampleStreaming : MonobitEngine.MonoBehaviour
    {
        /*
        #region PLEASE SET THESE VARIABLES IN THE INSPECTOR
        [Space(10)]
        [Tooltip("The service URL (optional). This defaults to \"https://stream.watsonplatform.net/speech-to-text/api\"")]
        [SerializeField]
        private string _serviceUrl;
        [Tooltip("Text field to display the results of streaming.")]
        public Text ResultsField;
        [Header("IAM Authentication")]
        [Tooltip("The IAM apikey.")]
        [SerializeField]
        private string _iamApikey;

        [Header("Parameters")]
        // https://www.ibm.com/watson/developercloud/speech-to-text/api/v1/curl.html?curl#get-model
        [Tooltip("The Model to use. This defaults to en-US_BroadbandModel")]
        [SerializeField]
        private string _recognizeModel;
        #endregion
                private int _recordingRoutine = 0;
        private string _microphoneID = null;
        private AudioClip _recording = null;
        private int _recordingBufferSize = 1;
        private int _recordingHZ = 22050;
        bool OnRecord = true;

                DateTime now;
            string timeStamp;

            private SpeechToTextService _service;
        */

        DateTime now;
        string timeStamp;

        [SerializeField]
        public Text text;
        [SerializeField]
        public GameObject[] Button;




        [SerializeField]
        public GameObject mun;


        //voice_Sample vS = new voice_Sample();

        // voicesampleからの移植
        bool Record = true;
        string filePath;
        string LogDataFilePath = @"/LogDatas/LogData.txt";       //Assets\以下の音声ファイルの書き込み先のファイル指定


        int NowBottonPushed = -1;


        private IMediaManager _mediaManager;



        void Start()
        {

            /*
            LogSystem.InstallDefaultReactors();
            Runnable.Run(CreateService());
            */

            //voicesampleからの移植
            //LogData i .txt と WadaiButton の初期化
            for (int i = 0; i < 8; i++)
            {
                //filePathのパス指定
                FilePathSelect(i);
                File.CreateText(filePath);
                if (i == 7)
                {
                    FilePathSelect(-1); break;
                }
            }
            filePath = Application.dataPath + @"/LogDatas/LogData.txt";
            File.CreateText(filePath);


            text.text = "話題未選択";
        }

        /*
        private void Update()
        {
            if (Record == false) StartRecording();
        }
        */

        //Watson時代の遺産
        /*
            private IEnumerator CreateService()
            {
                if (string.IsNullOrEmpty(_iamApikey))
                {
                    throw new IBMException("Plesae provide IAM ApiKey for the service.");
                }

                IamAuthenticator authenticator = new IamAuthenticator(apikey: _iamApikey);

                //  Wait for tokendata
                while (!authenticator.CanAuthenticate())
                    yield return null;

                _service = new SpeechToTextService(authenticator);
                if (!string.IsNullOrEmpty(_serviceUrl))
                {
                    _service.SetServiceUrl(_serviceUrl);
                }
                _service.StreamMultipart = true;

                Active = true;
                //StartRecording();
            }

            public bool Active
            {
                get { return _service.IsListening; }
                set
                {
                    if (value && !_service.IsListening)
                    {
                        _service.RecognizeModel = (string.IsNullOrEmpty(_recognizeModel) ? "ja-JP_BroadbandModel" : _recognizeModel);
                        _service.DetectSilence = true;
                        _service.EnableWordConfidence = true;
                        _service.EnableTimestamps = true;
                        _service.SilenceThreshold = 0.01f;
                        _service.MaxAlternatives = 0;
                        _service.EnableInterimResults = true;
                        _service.OnError = OnError;
                        _service.InactivityTimeout = -1;
                        _service.ProfanityFilter = false;
                        _service.SmartFormatting = true;
                        _service.SpeakerLabels = false;
                        _service.WordAlternativesThreshold = null;
                        _service.StartListening(OnRecognize, OnRecognizeSpeaker);
                    }
                    else if (!value && _service.IsListening)
                    {
                        _service.StopListening();
                    }
                }
            }

            /// <summary>
            /// publicに変えました
            /// </summary>
            public void StartRecording()
            {
                LogSystem.InstallDefaultReactors();
                Runnable.Run(CreateService());

                if (_recordingRoutine == 0)
                {
                    UnityObjectUtil.StartDestroyQueue();
                    _recordingRoutine = Runnable.Run(RecordingHandler());
                }
            }

            /// <summary>
            /// publicに変えました
            /// </summary>
            public void StopRecording()
            {
                if (_recordingRoutine != 0)
                {
                    //Microphone.End(_microphoneID);
                    Runnable.Stop(_recordingRoutine);
                    _recordingRoutine = 0;
                }
            }

            private void OnError(string error)
            {
                Active = false;

                Log.Debug("ExampleStreaming.OnError()", "Error! {0}", error);
            }

            private IEnumerator RecordingHandler()
            {
                Log.Debug("ExampleStreaming.RecordingHandler()", "devices: {0}", Microphone.devices);
                _recording = Microphone.Start(_microphoneID, true, _recordingBufferSize, _recordingHZ);
                //_recording = AC;
                yield return null;      // let _recordingRoutine get set..

                if (_recording == null)
                {
                    StopRecording();
                    yield break;
                }


                bool bFirstBlock = true;
                int midPoint = _recording.samples / 2;  //多分サンプリング周波数のことだと思う
                float[] samples = null;

                while (_recordingRoutine != 0 && _recording != null)
                {
                    int writePos = Microphone.GetPosition(_microphoneID);
                    if (writePos > _recording.samples || !Microphone.IsRecording(_microphoneID))
                    {
                        Log.Error("ExampleStreaming.RecordingHandler()", "Microphone disconnected.");

                        StopRecording();
                        yield break;
                    }

                    if ((bFirstBlock && writePos >= midPoint)
                      || (!bFirstBlock && writePos < midPoint))
                    {
                        // front block is recorded, make a RecordClip and pass it onto our callback.
                        samples = new float[midPoint];
                        _recording.GetData(samples, bFirstBlock ? 0 : midPoint);

                        AudioData record = new AudioData();
                        record.MaxLevel = Mathf.Max(Mathf.Abs(Mathf.Min(samples)), Mathf.Max(samples));
                        record.Clip = AudioClip.Create("Recording", midPoint, _recording.channels, _recordingHZ, false);
                        record.Clip.SetData(samples, 0);

                        _service.OnListen(record);

                        bFirstBlock = !bFirstBlock;
                    }
                    else
                    {
                        // calculate the number of samples remaining until we ready for a block of audio, 
                        // and wait that amount of time it will take to record.
                        int remaining = bFirstBlock ? (midPoint - writePos) : (_recording.samples - writePos);
                        float timeRemaining = (float)remaining / (float)_recordingHZ;

                        yield return new WaitForSeconds(timeRemaining);
                    }
                }
                yield break;
            }

            private void OnRecognize(SpeechRecognitionEvent result)
            {

                if (result != null && result.results.Length > 0)
                {
                    foreach (var res in result.results)
                    {
                        foreach (var alt in res.alternatives)
                        {
                            string text = string.Format("{0} ({1}, {2:0.00})\n", alt.transcript, res.final ? "Final" : "Interim", alt.confidence);

                            Log.Debug("ExampleStreaming.OnRecognize()", text);
                            ResultsField.text = text;
                            if(res.final)   Dataoutput(timeStamp, MonobitNetwork.playerName, alt.transcript, alt.confidence);
                        }
                        if (res.keywords_result != null && res.keywords_result.keyword != null)
                        {
                            foreach (var keyword in res.keywords_result.keyword)
                            {
                                Log.Debug("ExampleStreaming.OnRecognize()", "keyword: {0}, confidence: {1}, start time: {2}, end time: {3}", keyword.normalized_text, keyword.confidence, keyword.start_time, keyword.end_time);
                            }
                        }

                        if (res.word_alternatives != null)
                        {
                            foreach (var wordAlternative in res.word_alternatives)
                            {
                                Log.Debug("ExampleStreaming.OnRecognize()", "Word alternatives found. Start time: {0} | EndTime: {1}", wordAlternative.start_time, wordAlternative.end_time);
                                foreach (var alternative in wordAlternative.alternatives)
                                Log.Debug("ExampleStreaming.OnRecognize()", "\t word: {0} | confidence: {1}", alternative.word, alternative.confidence);
                            }
                        }
                    }
                    //vS.Dataoutput(text);
                }
            }

            private void OnRecognizeSpeaker(SpeakerRecognitionEvent result)
            {
                if (result != null)
                {
                    foreach (SpeakerLabelsResult labelResult in result.speaker_labels)
                    {
                        Log.Debug("ExampleStreaming.OnRecognizeSpeaker()", string.Format("speaker result: {0} | confidence: {3} | from: {1} | to: {2}", labelResult.speaker, labelResult.from, labelResult.to, labelResult.confidence));
                    }
                }
            }
        */


        //voicesampleからの移植
        //データの出力
        public void Dataoutput(string now, string myname, string text, double confidence)
        {
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine(now + " , " + myname + " , " + text + " , " + confidence.ToString()); //テキストファイルに音声認識結果テキストと、音声認識時間(ms)を書きこみ
            }
            //ログの書き出し
            //Debug.Log(text + " , " +confidence.ToString());
        }


        //filePathのパス指定
        public void FilePathSelect(int number)
        {
            switch (number)
            {
                case 0:
                    LogDataFilePath = @"/LogDatas/LogData0.txt";
                    filePath = Application.dataPath + LogDataFilePath;
                    text.text = Button[number].GetComponentInChildren<Text>().text + "を選択中";
                    break;
                case 1:
                    LogDataFilePath = @"/LogDatas/LogData1.txt";
                    filePath = Application.dataPath + LogDataFilePath;
                    text.text = Button[number].GetComponentInChildren<Text>().text + "を選択中";
                    break;
                case 2:
                    LogDataFilePath = @"/LogDatas/LogData2.txt";
                    filePath = Application.dataPath + LogDataFilePath;
                    text.text = Button[number].GetComponentInChildren<Text>().text + "を選択中";
                    break;
                case 3:
                    LogDataFilePath = @"/LogDatas/LogData3.txt";
                    filePath = Application.dataPath + LogDataFilePath;
                    text.text = Button[number].GetComponentInChildren<Text>().text + "を選択中";
                    break;
                case 4:
                    LogDataFilePath = @"/LogDatas/LogData4.txt";
                    filePath = Application.dataPath + LogDataFilePath;
                    text.text = Button[number].GetComponentInChildren<Text>().text + "を選択中";
                    break;
                case 5:
                    LogDataFilePath = @"/LogDatas/LogData5.txt";
                    filePath = Application.dataPath + LogDataFilePath;
                    text.text = Button[number].GetComponentInChildren<Text>().text + "を選択中";
                    break;
                case 6:
                    LogDataFilePath = @"/LogDatas/LogData6.txt";
                    filePath = Application.dataPath + LogDataFilePath;
                    text.text = Button[number].GetComponentInChildren<Text>().text + "を選択中";
                    break;
                case 7:
                    LogDataFilePath = @"/LogDatas/LogData7.txt";
                    filePath = Application.dataPath + LogDataFilePath;
                    text.text = Button[number].GetComponentInChildren<Text>().text + "を選択中";
                    break;
                default:
                    LogDataFilePath = @"/LogDatas/LogData.txt";
                    filePath = Application.dataPath + LogDataFilePath;
                    text.text = "話題未選択";
                    break;
            }
        }

        //話題ボタンが押されると呼び出されるメソッド
        public void WadaiButton0()
        {
            if (NowBottonPushed != 0)
            {
                FilePathSelect(0);
                Debug.Log("話題0が押されました");
                NowBottonPushed = 0;
            }
            else
            {
                FilePathSelect(-1);
                Debug.Log("話題が解除されました");
                NowBottonPushed = -1;
            }

        }
        public void WadaiButton1()
        {
            if (NowBottonPushed != 1)
            {
                FilePathSelect(1);
                Debug.Log("話題1が押されました");
                NowBottonPushed = 1;
            }
            else
            {
                FilePathSelect(-1);
                Debug.Log("話題が解除されました");
                NowBottonPushed = -1;
            }
        }
        public void WadaiButton2()
        {
            if (NowBottonPushed != 2)
            {
                FilePathSelect(2);
                Debug.Log("話題2が押されました");
                NowBottonPushed = 2;
            }
            else
            {
                FilePathSelect(-1);
                Debug.Log("話題が解除されました");
                NowBottonPushed = -1;
            }
        }
        public void WadaiButton3()
        {
            if (NowBottonPushed != 3)
            {
                FilePathSelect(3);
                Debug.Log("話題3が押されました");
                NowBottonPushed = 3;
            }
            else
            {
                FilePathSelect(-1);
                Debug.Log("話題が解除されました");
                NowBottonPushed = -1;
            }
        }
        public void WadaiButton4()
        {
            if (NowBottonPushed != 4)
            {
                FilePathSelect(4);
                Debug.Log("話題4が押されました");
                NowBottonPushed = 4;
            }
            else
            {
                FilePathSelect(-1);
                Debug.Log("話題が解除されました");
                NowBottonPushed = -1;
            }
        }
        public void WadaiButton5()
        {
            if (NowBottonPushed != 5)
            {
                FilePathSelect(5);
                Debug.Log("話題5が押されました");
                NowBottonPushed = 5;
            }
            else
            {
                FilePathSelect(-1);
                Debug.Log("話題が解除されました");
                NowBottonPushed = -1;
            }
        }
        public void WadaiButton6()
        {
            if (NowBottonPushed != 6)
            {
                FilePathSelect(6);
                Debug.Log("話題6が押されました");
                NowBottonPushed = 6;
            }
            else
            {
                FilePathSelect(-1);
                Debug.Log("話題が解除されました");
                NowBottonPushed = -1;
            }
        }
        public void WadaiButton7()
        {
            if (NowBottonPushed != 7)
            {
                FilePathSelect(7);
                Debug.Log("話題7が押されました");
                NowBottonPushed = 7;
            }
            else
            {
                FilePathSelect(-1);
                Debug.Log("話題が解除されました");
                NowBottonPushed = -1;
            }
        }




        //Recordボタンを押すと呼び出されるメソッド
        public void SetRecord()
        {
            /*
                //Recordがtrueなら(最初に押されたら)
                if (Record)
                {
                    StartRecording();   
                    Record = false;                                                    //Recordをfalseにする
                    Debug.Log("Record True");                                           //デバッグログ
                }
                //Recordがfalseなら(音声認識中に押されたら)
                else
                {
                    StopRecording();
                    Record = true;              //Recordをtrueにする
                    Debug.Log("Record False");  //デバッグログ
                }
                Debug.Log(MonobitNetwork.playerName);
                Debug.Log(now);
            */
        }



















        ///MainSceneMUNScriptからの移行
        ///

        [SerializeField]
        private Text RoomNameText;

        [SerializeField]
        private Text PlayerList;

        [SerializeField]
        private GameObject MuteLine;

        public bool Mute = false;


        /** ルーム名. */
        private string roomName = "";

        /** ルーム内のプレイヤーに対するボイスチャット送信可否設定. */
        private Dictionary<MonobitPlayer, Int32> vcPlayerInfo = new Dictionary<MonobitPlayer, int>();

        /** 自身が所有するボイスアクターのMonobitViewコンポーネント. */
        private MonobitVoice myVoice = null;

        private bool first = true;


        /** ボイスチャット送信可否設定の定数. */
        private enum EnableVC
        {
            ENABLE = 0,         /**< 有効. */
            DISABLE = 1,        /**< 無効. */
        }

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


        private void Update()
        {
            now = DateTime.Now;
            timeStamp = now.ToString("yyyy/MM/dd HH:mm:ss");
            //MUNサーバに接続している場合
            if (MonobitNetwork.isConnect)
            {
                // ルームに入室している場合
                if (MonobitNetwork.inRoom)
                {

                    roomName = MonobitNetwork.room.name;
                    RoomNameText.text = "roomName : " + roomName;
                    PlayerList.text = "PlayerList : ";


                    //Debug.Log("PlayerList:");
                    foreach (MonobitPlayer player in MonobitNetwork.playerList)
                    {
                        PlayerList.text = PlayerList.text + player.name + " ";
                    }


                    if (Mute)
                    {
                        List<MonobitPlayer> playerList = new List<MonobitPlayer>(vcPlayerInfo.Keys);
                        List<MonobitPlayer> vcTargets = new List<MonobitPlayer>();
                        foreach (MonobitPlayer player in playerList)
                        {
                            vcPlayerInfo[player] = (Int32)EnableVC.DISABLE;
                            Debug.Log("vcPlayerInfo[" + player + "] = " + vcPlayerInfo[player]);
                            Debug.Log(player.ToString());

                            // ボイスチャットの送信可のプレイヤー情報を登録する
                            if (vcPlayerInfo[player] == (Int32)EnableVC.ENABLE)
                            {
                                vcTargets.Add(player);
                            }
                        }
                        // ボイスチャットの送信可否設定を反映させる
                        myVoice.SetMulticastTarget(vcTargets.ToArray());
                    }
                }
            }
        }

        public void LeaveRoom()
        {
            MonobitNetwork.LeaveRoom();
            //Debug.Log("ルームから退出しました");
            //ここでスタートのシーンに遷移する
            SceneManager.LoadScene("StartScene");
        }

        /*
        private void OnGUI()
        {

            //MUNサーバに接続している場合
            if (MonobitNetwork.isConnect)
            {
                // ルームに入室している場合
                if (MonobitNetwork.inRoom)
                {
                    GUILayout.BeginHorizontal();
                    //Debug.Log(MonobitEngine.MonobitNetwork.room.name);
                    roomName = MonobitEngine.MonobitNetwork.room.name;
                    RoomNameText.text = "roomName : " + roomName;
                    GUILayout.Label("roomName : " + roomName, m_guiStyle);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("PlayerList : ", m_guiStyle);
                    PlayerList.text = "PlayerList : ";


                    //Debug.Log("PlayerList:");
                    foreach (MonobitPlayer player in MonobitNetwork.playerList)
                    {
                        PlayerList.text = PlayerList.text + player.name + " ";
                        GUILayout.Label(player.name + " ");
                        //Debug.Log(player.name + " ");
                    }
                    GUILayout.EndHorizontal();

                    // ルームからの退室
                    if (GUILayout.Button("Leave Room", m_guiStyle, GUILayout.Width(150)))
                    {
                        MonobitNetwork.LeaveRoom();
                        //Debug.Log("ルームから退出しました");
                        //ここでスタートのシーンに遷移する
                        SceneManager.LoadScene("StartScene");
                    }

                    if (myVoice != null)
                    {
                        // 送信タイプの設定
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("VoiceChat Send Type : ", m_guiStyle);
                        Int32 streamType = myVoice.SendStreamType == StreamType.BROADCAST ? 0 : 1;
                        myVoice.SendStreamType = (StreamType)GUILayout.Toolbar(streamType, new string[] { "broadcast", "multicast" }, m_guiStyle);
                        GUILayout.EndHorizontal();

                        // マルチキャスト送信の場合の、ボイスチャットの送信可否設定
                        if (myVoice.SendStreamType == StreamType.MULTICAST)
                        {
                            List<MonobitPlayer> playerList = new List<MonobitPlayer>(vcPlayerInfo.Keys);
                            List<MonobitPlayer> vcTargets = new List<MonobitPlayer>();
                            foreach (MonobitPlayer player in playerList)
                            {
                                // GUI による送信可否の切替
                                GUILayout.BeginHorizontal();
                                GUILayout.Label("PlayerName : " + player.name + " ", m_guiStyle);
                                GUILayout.Label("Send Permission: ", m_guiStyle);
                                vcPlayerInfo[player] = GUILayout.Toolbar(vcPlayerInfo[player], new string[] { "Allow", "Deny" }, m_guiStyle);
                                GUILayout.EndHorizontal();
                                // ボイスチャットの送信可のプレイヤー情報を登録する
                                if (vcPlayerInfo[player] == (Int32)EnableVC.ENABLE)
                                {
                                    vcTargets.Add(player);
                                }
                            }

                            // ボイスチャットの送信可否設定を反映させる
                            myVoice.SetMulticastTarget(vcTargets.ToArray());
                        }
                    }

                }
            }
        }
        */


        // 自身がルーム入室に成功したときの処理
        public void OnJoinedRoom()
        {
            vcPlayerInfo.Clear();
            vcPlayerInfo.Add(MonobitNetwork.player, (Int32)EnableVC.DISABLE);

            foreach (MonobitPlayer player in MonobitNetwork.otherPlayersList)
            {
                vcPlayerInfo.Add(player, (Int32)EnableVC.ENABLE);
            }

            GameObject go = MonobitNetwork.Instantiate("VoiceActor", Vector3.zero, Quaternion.identity, 0);
            myVoice = go.GetComponent<MonobitVoice>();
            AudioClip AC = go.GetComponent<AudioSource>().clip;


            if (myVoice != null)
            {
                myVoice.SetMicrophoneErrorHandler(OnMicrophoneError);
                myVoice.SetMicrophoneRestartHandler(OnMicrophoneRestart);
            }
        }

        // 誰かがルームにログインしたときの処理
        public void OnOtherPlayerConnected(MonobitPlayer newPlayer)
        {
            if (!vcPlayerInfo.ContainsKey(newPlayer))
            {
                vcPlayerInfo.Add(newPlayer, (Int32)EnableVC.ENABLE);
            }
        }

        // 誰かがルームからログアウトしたときの処理
        public virtual void OnOtherPlayerDisconnected(MonobitPlayer otherPlayer)
        {
            if (vcPlayerInfo.ContainsKey(otherPlayer))
            {
                vcPlayerInfo.Remove(otherPlayer);
            }
        }

        /// <summary>
        /// マイクのエラーハンドリング用デリゲート
        /// </summary>
        /// <returns>
        /// true : 内部にてStopCaptureを実行しループを抜けます。
        /// false: StopCaptureを実行せずにループを抜けます。
        /// </returns>
        public bool OnMicrophoneError()
        {
            UnityEngine.Debug.LogError("Error: Microphone Error !!!");
            return true;
        }

        /// <summary>
        /// マイクのリスタート用デリゲート
        /// </summary>
        /// <remarks>
        /// 呼び出された時点ではすでにStopCaptureされています。
        /// </remarks>
        public void OnMicrophoneRestart()
        {
            UnityEngine.Debug.LogWarning("Info: Microphone Restart !!!");
        }

        public void muteButtonOnclicked()
        {
            //MUNサーバに接続している場合
            if (MonobitNetwork.isConnect)
            {
                // ルームに入室している場合
                if (MonobitNetwork.inRoom)
                {
                    Mute = !Mute;
                    if (Mute)
                    {
                        myVoice.SendStreamType = StreamType.MULTICAST;
                        MuteLine.SetActive(true);　
                    }

                    else
                    {
                        myVoice.SendStreamType = StreamType.BROADCAST;
                        MuteLine.SetActive(false);

                    }
                }
            }
        }

        public void OnclickedDebugButoon()
        {
            GameObject _DebugButton = GameObject.Find("DebugButton");
            
            /*
            GCSpeechRecognition GC = new GCSpeechRecognition();
            GC.MUNAudioClipAccess(AC);
            GC.StartRecord(false);
            */
        }
    }
}