using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�j�w�P�ɻݨD������
[RequireComponent(typeof(CanvasGroup))]
public class UIPanelCtrl : MonoBehaviour
{

    private CanvasGroup CG;

    public bool isOn { get; private set; }

    //�}���Ұ�
    private void OnEnable()
    {
        //��� CanvasGroup ����
        CG = GetComponent<CanvasGroup>();
        //�w�]��������
        Switch(false);
    }

    /// <summary>
    /// ���O�}������
    /// </summary>
    /// <param name="B">�}:true / ��:false</param>
    public void Switch(bool B)
    {
        isOn = B;
        //�p�G(�})
        if (isOn)
        {
            CG.alpha = 1;
            CG.blocksRaycasts = true;
        }
        else //�_�h
        {
            CG.alpha = 0;
            CG.blocksRaycasts = false;
        }
    }
   
}
