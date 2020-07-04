using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class voiceSample : MonoBehaviour
{
    private PXCMSession session;
    private PXCMAudioSource source;
    private PXCMSpeechRecognition sr;
    private PXCMSpeechRecognition.Handler handler;

    public void Start()
    {
        session = PXCMSession.CreateInstance();
        source = session.CreateAudioSource();

        PXCMAudioSource.DeviceInfo dinfo = null;

        //デバイスを検出して出力
        source.QueryDeviceInfo(0, out dinfo);
        source.SetDevice(dinfo);
        Debug.Log(dinfo.name);

        session.CreateImpl<PXCMSpeechRecognition>(out sr);

        PXCMSpeechRecognition.ProfileInfo pinfo;
        sr.QueryProfile(out pinfo);
        pinfo.language = PXCMSpeechRecognition.LanguageType.LANGUAGE_JP_JAPANESE;
        sr.SetProfile(pinfo);

        handler = new PXCMSpeechRecognition.Handler();
        handler.onRecognition = (x) => Debug.Log(x.scores[0].sentence);
        sr.SetDictation();
        sr.StartRec(source, handler);
    }

    void OnDisable()
    {
        if (sr != null)
        {
            sr.StopRec();
            sr.Dispose();
        }

        if (session != null)
            session.Dispose();
    }
}
