using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Setting : MonoBehaviour
{
    public AudioMixer audioMixer;

    public AudioMixer effect;

    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("Volume", volume);   
    }
    public void SetEffect(float vl)
    {
        effect.SetFloat("Effect", vl);
    }
}
