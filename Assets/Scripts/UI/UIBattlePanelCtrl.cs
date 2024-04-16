using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIBattlePanelCtrl : MonoBehaviour
{
    private UIPanelCtrl _panelCtrl;
    private UIPanelCtrl panelCtrl
    {
        get
        {
            if(_panelCtrl == null) _panelCtrl = GetComponent<UIPanelCtrl>();
            return _panelCtrl;
        }
    }


    [Header("�԰�����Ʈw")]
    public RumbleDB rumbleDB;
    [Header("�԰������UI�����")]
    public UIRumbleInfoCtrl[] infoCtrls = new UIRumbleInfoCtrl[4];

    [Header("��q��TUI����")]
    public Image eBarImg;
    public TextMeshProUGUI eBarText;

    private string eBarString
    {
        get 
        {
            return GameSystem.energy.ToString("F0");
        }
    }

    private float eBarFillamount
    {
        get
        {
            return GameSystem.energy / 10f;
        }
    }
    private Action rumbleInfoLockUI;

    private void OnEnable()
    {
        GameSystem.SetBattlePanelUI(this);//�t�ΰU��
        for (int i = 0; i < infoCtrls.Length; i++)
        {//1��h���\��j�w
            rumbleInfoLockUI += infoCtrls[i].LockUI;
        }
          
    }
    private void OnDisable()
    {
        GameSystem.SetBattlePanelUI(null);//�Ѱ��U��
        for (int i = 0; i < infoCtrls.Length; i++)
        {//1��h���\��Ѹj
            rumbleInfoLockUI -= infoCtrls[i].LockUI;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateRumbleList();
    }

    // Update is called once per frame
    void Update()
    {
        if (!panelCtrl.isOn) return;

        //��q��_
        GameSystem.energy += Time.deltaTime;
        eBarText.text = eBarString;
        eBarImg.fillAmount = eBarFillamount;
        //���K��s�԰����UI�M��
        rumbleInfoLockUI();
    }
    /// <summary>
    /// �I�s�t�ζ}�l�԰�
    /// </summary>
    public void StartBattle()
    {
        GameSystem.StartBattle();
    }

    public void UpdateRumbleList()
    {
        for (int i = 0; i < 4; i++)
        {
            if (infoCtrls[i].noData)
            {
                infoCtrls[i].UpdateUI(rumbleDB.RandomRumble());
            }           
        }
    }



}
