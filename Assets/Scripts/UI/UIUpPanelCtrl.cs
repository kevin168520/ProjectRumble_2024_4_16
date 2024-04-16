using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIUpPanelCtrl : MonoBehaviour
{
    [Header("�U�Կ��ʵe���")]
    public Animator aniDropDown;
    [Header("�p�ɾ���r")]
    public TextMeshProUGUI textTimer;
    [Header("�Ĥ�HP BAR")]
    public Image barP2;
    [Header("�ڤ�HP BAR")]
    public Image barP1;

    private float interval = 0;
    private int timer
    {

        get
        {
            return GameSystem.timer;
        }
        set
        {
            GameSystem.timer = value;
        }
    }
    private string timeStr
    {
        get
        {
            //�^�� �ɶ���� timer��60  = > ����:�l��
            return $"{(timer / 60).ToString("F0")}:{(timer % 60).ToString("00")}";
        }
    }

    private bool lose
    {
        get
        {
            return timer <= 0;
        }
    }

    private void OnEnable()
    {
        GameSystem.SetUpPanelCtrl(this);
        //��s�p�ɤ�r
        textTimer.text = timeStr;
    }

    private void OnDisable()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameSystem.inBattle) return;
        UPdateTimer();
    }

    /// <summary>
    /// ���a�D��HP HUD(���Y���UI)
    /// </summary>
    /// <param name="val">�ʤ��I</param>
    public void HpBarCtrlP1(float val)
    {
        barP1.fillAmount = val;
        //PLAYER HP �k�s
        if (barP1.fillAmount <= 0) GameSystem.GameOverInfo();
    }

    /// <summary>
    /// �Ĥ�D��HP HUD(���Y���UI)
    /// </summary>
    /// <param name="val">�ʤ��I</param>
    public void HpBarCtrlP2(float val)
    {
        barP2.fillAmount = val;
        //Boss HP �k�s(���)
        if (barP2.fillAmount <= 0)
        {
            GameSystem.GameOverInfo();                   
        }
    }

    /// <summary>
    /// ��s�p�ɾ�
    /// </summary>
    void UPdateTimer()
    {
        if (!lose && interval >= 1)
        {//�ɶ������� && ���j1��
            timer--;//�p�ɭ˼�
            interval = 0;//���m���j
            //�p�ɾ��k�s(�ɶ���)
            if (timer <= 0) GameSystem.GameOverInfo();
        }
        else
        {//�p�ⶡ�j�ɶ�
            interval += Time.deltaTime;
        }
        //��s�p�ɾ���r
        textTimer.text = timeStr;
    }

    public void DropDownSwitch(bool B)
    {
        //�C������ɶ��`����(�}�� ? �Ȱ� : 1����)����
        Time.timeScale = B ? 0 : 1;
        //����ʵe�ɭ� (�}�� ? �Y : �� ) ����
        aniDropDown.Play("UpPanel", 0, B ? 0 : 1);
        //����ʵe�ɳt�� (�}�� ? �� : �f ) ����
        aniDropDown.SetFloat("Speed", B ? 2 : -5);       
    }

    /// <summary>
    /// �������d
    /// </summary>
    public void ReloadStage()
    {
        Time.timeScale = 1;
        GameSystem.ReloadScene();
        GameSystem.ClearTargetSystem();
    }
    /// <summary>
    /// ���}���d
    /// </summary>
    public void ExitStage()
    {
        Time.timeScale = 1;
        GameSystem.ToSceneByName("Menu");
        GameSystem.ClearTargetSystem();
    }
}
