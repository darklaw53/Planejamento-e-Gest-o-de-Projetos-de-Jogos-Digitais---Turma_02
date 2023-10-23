using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
    public AudioMixer masterMixer;
    bool toggle;
    public Image image;

    private void Awake()
    {
        float x = 0;
        masterMixer.GetFloat("Volume", out x);
        if (x == -80)
        {
            image.color = Color.gray;
            toggle = !toggle;
        }
    }

    public void Toggle()
    {
        toggle = !toggle;

        if (toggle)
        {
            image.color = Color.gray;
            masterMixer.SetFloat("Volume", -80);
        }
        else
        {
            image.color = Color.white;
            masterMixer.SetFloat("Volume", 0);
        }
    }
}
