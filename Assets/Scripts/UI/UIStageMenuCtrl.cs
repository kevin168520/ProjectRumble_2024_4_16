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
        //�s���ܹC���t��:�U��
        GameSystem.SetStageMenuCtrl(this);
    }

   /// <summary>
   /// ��s���D���
   /// </summary>
   /// <param name="header">���D���</param>
   public void UpdateHeader(string header)
    {
        textHeader.text = header;
    }

    public void UpdateMenu(List<LevelData> levels) 
    {
        //��s(���s)��T : �j��(�_�l�� ;���I��; �W��)
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
