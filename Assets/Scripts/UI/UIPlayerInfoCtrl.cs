using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInfoCtrl : MonoBehaviour
{
    [Header("經驗值光棒元件")]
    public Image expBar;

    private void OnEnable()
    {
        expBar.fillAmount = GameSystem.expFillAmount;
    }
}
