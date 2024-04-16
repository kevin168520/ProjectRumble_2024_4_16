using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIGameOverPanelCtrl : MonoBehaviour
{
    private UIPanelCtrl _panelCtrl;
    private UIPanelCtrl panelCtrl
    {
        get
        {
            if (_panelCtrl == null) _panelCtrl = GetComponent<UIPanelCtrl>();
            return _panelCtrl;
        }
    }

    public TextMeshProUGUI textGameOver;

    private void OnEnable()
    {
        GameSystem.SetGameOverPanel(this);
        //panelCtrl.Switch(false);
    }

    public void ShowInfo()
    {
        panelCtrl.Switch(true);
        textGameOver.text = GameSystem.win ? "勝利" : "失敗";
    }

    /// <summary>
    /// 重載關卡
    /// </summary>
    public void ReloadStage()
    {
        GameSystem.ClearTargetSystem();
        GameSystem.ReloadScene();
    }
    /// <summary>
    /// 離開關卡
    /// </summary>
    public void ExitStage()
    {
        GameSystem.ClearTargetSystem();
        GameSystem.ToSceneByName("Menu");
    }
}
