using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct UIMenu 
{
    public Image icon;
    public Image iconInfo;
    public Button button;

    public TextMeshProUGUI textlvName;
    public TextMeshProUGUI textlv;
    public TextMeshProUGUI textInfo;

}
public class UIStageMenuCtrl : MonoBehaviour
{
    public TextMeshProUGUI textHeader;
    public List<UIMenu> UIMenuList = new List<UIMenu>(5);

    private void OnEnable()
    {
        //連結至遊戲系統:託管
        GameSystem.SetStageMenuCtrl(this);
    }

   /// <summary>
   /// 更新標題文案
   /// </summary>
   /// <param name="header">標題文案</param>
   public void UpdateHeader(string header)
    {
        textHeader.text = header;
    }

    public void UpdateMenu(List<LevelData> levels) 
    {
        //更新(按鈕)資訊 : 迴圈(起始值 ;終點值; 增值)
        for (int i = 0; i < 5; i++)
        {
            UIMenuList[i].icon.sprite = levels[i].icon;
            UIMenuList[i].textlvName.text = levels[i].lvName;
            UIMenuList[i].textlv.text = levels[i].lv.ToString();
            UIMenuList[i].textInfo.text = levels[i].lvFactor.ToString();
            UIMenuList[i].button.interactable = levels[i].lvFactor < GameSystem.lvMileage;
        }

    }
}
