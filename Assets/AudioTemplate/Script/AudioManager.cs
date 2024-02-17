using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider MasterSlider;
    [SerializeField] Slider BGMSlider;
    [SerializeField] Slider SESlider;

    public bool entryFlag
    {
        get; private set;
    } = false;

    // Start is called before the first frame update
    void Start()
    {
        audioMixer.GetFloat("Master", out float masterValue);
        audioMixer.GetFloat("BGM", out float bgmValue);
        audioMixer.GetFloat("SE", out float seValue);

        MasterSlider.value = ConvertVolume2linear(masterValue);
        BGMSlider.value = ConvertVolume2linear(bgmValue);
        SESlider.value = ConvertVolume2linear(seValue);

        entryFlag = true;
    }

    public void SetVolume()
    {
        if (!entryFlag)
        {
            return;
        }

        Debug.Log("‰¹—Ê’²®");
        audioMixer.SetFloat("Master", ConvertVolume2dB(MasterSlider.value));
        audioMixer.SetFloat("BGM", ConvertVolume2dB(BGMSlider.value));
        audioMixer.SetFloat("SE", ConvertVolume2dB(SESlider.value));
    }

    float ConvertVolume2dB(float volume) => Mathf.Clamp(20f * Mathf.Log10(Mathf.Clamp(volume, 0f, 1f)), -80f, 0f);
    float ConvertVolume2linear(float decibel) => Mathf.Clamp(Mathf.Pow(10, decibel / 20f), 0f, 1f);
}
