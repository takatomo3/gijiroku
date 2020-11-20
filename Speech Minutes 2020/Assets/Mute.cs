using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mute : MonoBehaviour
{

    public void OnClickMuteButton()
    {
        AudioListener.volume = 0;
    }

}
