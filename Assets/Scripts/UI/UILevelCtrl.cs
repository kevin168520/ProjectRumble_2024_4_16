using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILevelCtrl : MonoBehaviour
{
    [Header("���d���{����")]
    public TextMeshProUGUI levelText;

    private void OnEnable()
    {
        levelText.text = GameSystem.lvMileageStr;
    }
}
