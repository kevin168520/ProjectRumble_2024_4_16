using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICurrencyCtrl : MonoBehaviour
{
    [Header("°]³f¤å¦r¤¸¥ó")]
    public TextMeshProUGUI currencyText;
    private void OnEnable()
    {
        currencyText.text = GameSystem.money.ToString();
    }
}
