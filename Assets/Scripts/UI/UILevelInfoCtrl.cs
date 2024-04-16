using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UILevelInfoCtrl : UIPanelCtrl
{
    public TextMeshProUGUI textlvName;
    public TextMeshProUGUI textlvDes;

    // Start is called before the first frame update
    void Start()
    {
        GameSystem.SetLevelInfoCtrl(this);
    }

    public void UpdateInfo(LevelData data)
    {
        textlvName.text = data.lvName;
        textlvDes.text = data.lvDes;
        Switch(true);
    }

    public void LevelStart()
    {
        GameSystem.LevelStart();
    }
}