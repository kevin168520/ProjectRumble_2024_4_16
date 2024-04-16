using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILevelCtrl : MonoBehaviour
{
    [Header("關卡里程元件")]
    public TextMeshProUGUI levelText;

    private void OnEnable()
    {
        levelText.text = GameSystem.lvMileageStr;
    }
}
