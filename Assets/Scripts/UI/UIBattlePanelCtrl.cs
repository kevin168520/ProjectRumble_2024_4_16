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


    [Header("戰鬥單位資料庫")]
    public RumbleDB rumbleDB;
    [Header("戰鬥單位資料UI元件組")]
    public UIRumbleInfoCtrl[] infoCtrls = new UIRumbleInfoCtrl[4];

    [Header("能量資訊UI元件")]
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
        GameSystem.SetBattlePanelUI(this);//系統託管
        for (int i = 0; i < infoCtrls.Length; i++)
        {//1對多的功能綁定
            rumbleInfoLockUI += infoCtrls[i].LockUI;
        }
          
    }
    private void OnDisable()
    {
        GameSystem.SetBattlePanelUI(null);//解除託管
        for (int i = 0; i < infoCtrls.Length; i++)
        {//1對多的功能解綁
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

        //能量恢復
        GameSystem.energy += Time.deltaTime;
        eBarText.text = eBarString;
        eBarImg.fillAmount = eBarFillamount;
        //順便刷新戰鬥單位UI清單
        rumbleInfoLockUI();
    }
    /// <summary>
    /// 呼叫系統開始戰鬥
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
