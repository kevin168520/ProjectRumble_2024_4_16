using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICurrencyCtrl : MonoBehaviour
{
    [Header("�]�f��r����")]
    public TextMeshProUGUI currencyText;
    private void OnEnable()
    {
        currencyText.text = GameSystem.money.ToString();
    }
}
