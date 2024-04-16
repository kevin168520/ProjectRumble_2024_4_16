using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCtrl : MonoBehaviour
{
    
    private List<Transform> points;
    /// <summary>
    /// �̫�@���I���s��
    /// </summary>
    public int lastIndex
    {
        get 
        {
            return points.Count - 1;
        }
    }

    private void OnEnable()
    {
        //���|�I�M�� = �إ߲M��(���X����) �ഫ
        points = new List<Transform>(GetComponentsInChildren<Transform>());
        //�w�]�|�N�ۨ���� : �ҥH�����Ĥ@�Ӫ���
        points.RemoveAt(0);
    }

    /// <summary>
    /// �ϥΧǦC�����o���|�I����
    /// </summary>
    /// <param name="index">�ǦC��</param>
    /// <returns>���|�I����</returns>
    public Transform GetPoint(int index)
    {
        //�ǦC��0�H�W�B�p��M���`�� ? �^���I���� : �L
        return index >= 0 && index < points.Count 
            ? points[index] : null;
    }
}
