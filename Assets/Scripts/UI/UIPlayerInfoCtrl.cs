using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInfoCtrl : MonoBehaviour
{
    [Header("�g��ȥ��Τ���")]
    public Image expBar;

    private void OnEnable()
    {
        expBar.fillAmount = GameSystem.expFillAmount;
    }
}
