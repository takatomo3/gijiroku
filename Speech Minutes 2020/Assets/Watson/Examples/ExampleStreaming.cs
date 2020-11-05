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
using IBM.Watson.SpeechToText.V1;
using IBM.Cloud.SDK;
using IBM.Cloud.SDK.Authentication;
using IBM.Cloud.SDK.Authentication.Iam;
using IBM.Cloud.SDK.Utilities;
using IBM.Cloud.SDK.DataTypes;
using System.IO;


namespace IBM.Watsson.Examples
{
    public class ExampleStreaming : MonoBehaviour
    {
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
        [SerializeField]
        public Text text;
        [SerializeField]
        public GameObject[] Button;


        private int _recordingRoutine = 0;
        private string _microphoneID = null;
        private AudioClip _recording = null;
        private int _recordingBufferSize = 1;
        private int _recordingHZ = 22050;
        bool OnRecord = true;
        bool isOnce = false;

        //voice_Sample vS = new voice_Sample();

        // voicesampleからの移植
        bool Record = true;
        string filePath;
        string LogDataFilePath = @"/LogDatas/LogData.txt";       //Assets\以下の音声ファイルの書き込み先のファイル指定


        int NowBottonPushed = -1;



        private SpeechToTextService _service;

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

        private void Update()
        {
            if (Record == false) StartRecording();
        }

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
            if (isOnce)
            {
                LogSystem.InstallDefaultReactors();
                Runnable.Run(CreateService());
                isOnce = false;
            }
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
                Microphone.End(_microphoneID);
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
                        if(res.final)   Dataoutput(alt.transcript , alt.confidence);
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


        //voicesampleからの移植
        //データの出力
        public void Dataoutput(string text, double confidence)
        {
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine(text + " , " + confidence.ToString()); //テキストファイルに音声認識結果テキストと、音声認識時間(ms)を書きこみ
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
            //Recordがtrueなら(最初に押されたら)
            if (Record)
            {
                isOnce = true;
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
        }

    }
}
