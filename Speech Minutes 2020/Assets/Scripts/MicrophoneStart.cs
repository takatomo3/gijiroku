using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine.VoiceChat;
using MonobitEngine;

public class MicrophoneStart : MonobitEngine.MonoBehaviour
{

    public void Micro()
    {
        AudioClip AC;
        Microphone.End(null);
        AC = Microphone.Start(null,true,1,22050);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
