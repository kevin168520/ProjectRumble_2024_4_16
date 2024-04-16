using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //靜態本體
    private static SoundManager _instance;
    public static SoundManager instance
    {
        get
        {//自動產生音效管理者：如果不存在
            if (_instance == null)
            {
                _instance = new GameObject("SoundManager").
                    AddComponent<SoundManager>();
            }
            return _instance;
        }
    }
    /// <summary>
    /// 音樂控制器
    /// </summary>
    private AudioSource _BGM;
    public AudioSource BGM
    {
        get
        {//自動產生並放置於管理者下
            if (_BGM == null) _BGM = new GameObject("BGM").
                    AddComponent<AudioSource>();
            _BGM.transform.SetParent(transform);
            return _BGM;
        }
    }
    /// <summary>
    /// 音樂音量
    /// </summary>
    public float volBGM
    {
        get
        {//系統讀取數值
            return PlayerPrefs.GetFloat("volBGM", 0.5f);
        }
        set
        {//系統設定數值
            PlayerPrefs.SetFloat("volBGM", value);
            PlayerPrefs.Save();
        }
    }
    /// <summary>
    /// 音效音量
    /// </summary>
    public float volSFX
    {
        get
        {//系統讀取數值
            return PlayerPrefs.GetFloat("volSFX", 0.5f);
        }
        set
        {//系統設定數值
            PlayerPrefs.SetFloat("volSFX", value);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// 音效控制器
    /// </summary>
    private AudioSource _SFX;
    public AudioSource SFX
    {
        get
        {//自動產生並放置於管理者下
            if (_SFX == null) _SFX = new GameObject("SFX").
                    AddComponent<AudioSource>();
            _SFX.transform.SetParent(transform);
            return _SFX;
        }
    }

    /// <summary>
    /// 播放背景音樂
    /// </summary>
    /// <param name="clip">音檔</param>
    public void PlayBGM(AudioClip clip)
    {
        BGM.clip = clip;//放入音檔
        BGM.loop = true;//循環
        BGM.volume = volBGM;//同步音量
        BGM.Play();//播放
    }
    /// <summary>
    /// 設定背景音樂音量
    /// </summary>
    /// <param name="vol">音量</param>
    public void SetVolBGM(float vol)
    {
        //volBGM = vol;
        PlayerPrefs.SetFloat("volBGM", vol);
        BGM.volume = volBGM;
    }

    /// <summary>
    /// 播放音效(一次性，可重疊)
    /// </summary>
    /// <param name="clip">音檔</param>
    public void PlaySFX(AudioClip clip)
    {
        SFX.volume = volSFX;//同步音量
        SFX.PlayOneShot(clip);//一次性播放
    }
    /// <summary>
    /// 設定音效音量
    /// </summary>
    /// <param name="vol">音量</param>
    public void SetVolSFX(float vol)
    {
        //volSFX = vol;
        PlayerPrefs.SetFloat("volSFX", vol);
        SFX.volume = volSFX;
    }
}