using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIRumbleInfoCtrl : MonoBehaviour
{
    [Header("�԰�����TUI����")]
    public Image iconRumbleImg;
    public TextMeshProUGUI eReqText;

    /// <summary>
    /// �O�_���԰�������ˬd���f
    /// </summary>
    public bool noData
    {
        get 
        {
            return data.NoData();
        }
    }
    public bool NoData()
    {
        return data.NoData();
    }


    private Toggle _toggle;
    private Toggle toggle
    {
        get 
        {
            if(_toggle == null) _toggle = GetComponent<Toggle>();
            return _toggle;
        }
    }
    /// <summary>
    /// �԰������
    /// </summary>
    public RumbleData data { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    /// <summary>
    /// �ϥξ԰������(�ϥΫ�Y��M��)
    /// </summary>
    public void UseData()
    {
        //�{�ɫŧi(���X���c�����Ѽ�)
        RumbleData data = this.data;
        //�ާ@���c�Ѽ�(�M��)
        data.ClearData();
        //�ާ@���G��^�h
        this.data = data;
        //����
        toggle.isOn = false;
    }


    /// <summary>
    /// ��s����ƨ�UI���
    /// </summary>
    /// <param name="rumbleData">�԰������</param>
    public void UpdateUI(RumbleData rumbleData)
    {
        data = rumbleData;//��Ƭ���
        toggle.isOn = false;//�w�]����
        //�I������(BackGround:�]����)
        toggle.image.sprite = data.rumbleIcon;
        //�����(CheckMark:�]����)
        iconRumbleImg.sprite = data.rumbleIcon;
        //��s��q�ݨD��T
        eReqText.text = data.rumbleReq.ToString();
    }

    /// <summary>
    /// ��q�O�_��������(����UI)
    /// </summary>
    public void LockUI()
    {
        //������w(�~���]�w�ܬ�:��q����)
        toggle.interactable = GameSystem.energy >= data.rumbleReq;
        //Icon��w(�ܷt)
        /*toggle.graphic.color = GameSystem.energy >= data.rumbleReq ?
            Color.white : Color.gray;*/
    }

    public void SelectRumble(bool B)
    {
        if (B)
        {//UI�Q�I�G:����������԰������UI
            GameSystem.SelectRumble(this);
        }
        else
        {//UI�Q����:��������������԰������UI
            GameSystem.UnSelectRumble();
        }
    }

}
