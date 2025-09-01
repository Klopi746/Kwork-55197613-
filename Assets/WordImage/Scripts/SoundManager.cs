using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class SoundManager : MonoBehaviour
{
    public AudioListener audioListenerp;
    public Image offSound;
    public bool flag = true;

    public Slider volumeSlider;

    private void Start()
    {
        if (YG2.saves.volumeSound == 0)
        {
            volumeSlider.value = 0.5f;

        }
        else
        {
            volumeSlider.value = YG2.saves.volumeSound;
        }



        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void OffOnSound()
    {
        //Debug.Log("OffOnSound");
        if (flag)
        {
            offSound.gameObject.SetActive(true);
            AudioListener.volume = 0;
            flag = false;

        }
        else
        {
            offSound.gameObject.SetActive(false);
            AudioListener.volume = 1;
            flag = true;
        }

    }

    public void SetVolume(float volume)
    {
        Debug.Log(volume);
        AudioListener.volume = volume;
        YG2.saves.volumeSound = volume;

    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        YG2.SaveProgress();
        //Debug.Log("OnDisable");
    }
}
