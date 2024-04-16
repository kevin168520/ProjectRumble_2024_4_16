using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettingPanelCtrl : MonoBehaviour
{
    public Slider sliderBGM;
    public Slider sliderSFX;

    private void OnEnable()
    {
        //同步音量
        sliderBGM.value = SoundManager.instance.volBGM;
        sliderSFX.value = SoundManager.instance.volSFX;
    }

    public void SetVolBGM(float vol)
    {
        SoundManager.instance.SetVolBGM(vol);
    }
    public void SetVolSFX(float vol)
    {
        SoundManager.instance.SetVolSFX(vol);
    }
}
