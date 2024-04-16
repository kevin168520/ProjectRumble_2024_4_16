using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //�R�A����
    private static SoundManager _instance;
    public static SoundManager instance
    {
        get
        {//�۰ʲ��ͭ��ĺ޲z�̡G�p�G���s�b
            if (_instance == null)
            {
                _instance = new GameObject("SoundManager").
                    AddComponent<SoundManager>();
            }
            return _instance;
        }
    }
    /// <summary>
    /// ���ֱ��
    /// </summary>
    private AudioSource _BGM;
    public AudioSource BGM
    {
        get
        {//�۰ʲ��ͨé�m��޲z�̤U
            if (_BGM == null) _BGM = new GameObject("BGM").
                    AddComponent<AudioSource>();
            _BGM.transform.SetParent(transform);
            return _BGM;
        }
    }
    /// <summary>
    /// ���֭��q
    /// </summary>
    public float volBGM
    {
        get
        {//�t��Ū���ƭ�
            return PlayerPrefs.GetFloat("volBGM", 0.5f);
        }
        set
        {//�t�γ]�w�ƭ�
            PlayerPrefs.SetFloat("volBGM", value);
            PlayerPrefs.Save();
        }
    }
    /// <summary>
    /// ���ĭ��q
    /// </summary>
    public float volSFX
    {
        get
        {//�t��Ū���ƭ�
            return PlayerPrefs.GetFloat("volSFX", 0.5f);
        }
        set
        {//�t�γ]�w�ƭ�
            PlayerPrefs.SetFloat("volSFX", value);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// ���ı��
    /// </summary>
    private AudioSource _SFX;
    public AudioSource SFX
    {
        get
        {//�۰ʲ��ͨé�m��޲z�̤U
            if (_SFX == null) _SFX = new GameObject("SFX").
                    AddComponent<AudioSource>();
            _SFX.transform.SetParent(transform);
            return _SFX;
        }
    }

    /// <summary>
    /// ����I������
    /// </summary>
    /// <param name="clip">����</param>
    public void PlayBGM(AudioClip clip)
    {
        BGM.clip = clip;//��J����
        BGM.loop = true;//�`��
        BGM.volume = volBGM;//�P�B���q
        BGM.Play();//����
    }
    /// <summary>
    /// �]�w�I�����֭��q
    /// </summary>
    /// <param name="vol">���q</param>
    public void SetVolBGM(float vol)
    {
        //volBGM = vol;
        PlayerPrefs.SetFloat("volBGM", vol);
        BGM.volume = volBGM;
    }

    /// <summary>
    /// ���񭵮�(�@���ʡA�i���|)
    /// </summary>
    /// <param name="clip">����</param>
    public void PlaySFX(AudioClip clip)
    {
        SFX.volume = volSFX;//�P�B���q
        SFX.PlayOneShot(clip);//�@���ʼ���
    }
    /// <summary>
    /// �]�w���ĭ��q
    /// </summary>
    /// <param name="vol">���q</param>
    public void SetVolSFX(float vol)
    {
        //volSFX = vol;
        PlayerPrefs.SetFloat("volSFX", vol);
        SFX.volume = volSFX;
    }
}