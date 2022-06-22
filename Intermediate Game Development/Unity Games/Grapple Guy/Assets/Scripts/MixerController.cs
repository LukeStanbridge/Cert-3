using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer menuAudioMixer;

    public void SetVolume(float sliderValue)
    {
        menuAudioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
    }
}
