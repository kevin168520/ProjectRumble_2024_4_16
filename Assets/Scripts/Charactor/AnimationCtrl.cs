using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationCtrl : MonoBehaviour
{
    #region �򥻤���
    /// <summary>
    /// Animator �������
    /// </summary>
    private Animator _animator;
    /// <summary>
    /// ��~�鷺�I�s�Τ��}���f(��Ū)
    /// </summary>
    public Animator animator
    {
        get 
        {
            if (_animator == null)//�Ĥ@���I�s�ɧ��
                _animator = GetComponent<Animator>();
            return _animator;
        }
    }

    /// <summary>
    /// �����ʧ@�ɤ��q
    /// </summary>
    private AnimationClip _attackClip;
    private AnimationClip attackClip
    {
        get 
        {
            if (_attackClip == null)
            {
                foreach (AnimationClip clip in 
                    animator.runtimeAnimatorController.animationClips)
                {//�M��(���X���󱽴y)
                    if (clip.name == "Attack")
                    {
                        _attackClip = clip;
                        break;
                    }
                }
            }
            return _attackClip; 
        }
    }
    #endregion �򥻤���

    /// <summary>
    /// �����ƥ�(�e�U)
    /// </summary>
    private Action<float> attackAction;
    private Action skillAction;
    private float attackPower;


    /// <summary>
    /// �]�wAnimation Type�ʵe���A��
    /// </summary>
    /// <param name="type">�ʵe����(���A)</param>
    public void SetAniType(AniType type)
    {
        //������ Idle �M Move ����
        animator.SetBool("Move", type == AniType.Move);
    }

    /// <summary>
    /// Ĳ�o���ʵe(�@���ʼ���)
    /// </summary>
    /// <param name="type">�ʵe����</param>
    public void SetTrigger(AniType type)
    {
        animator.SetTrigger(type.ToString());
    }

    public void SetAttackSpeed(float attackTime = 1f)
    {
        animator.SetFloat("AttackSpeed", attackClip.length / attackTime);
        /* AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == "Attack")
            {
                animator.SetFloat("AttackSpeed", clip.length / attackTime);
            }
        }*/
    }
    /// <summary>
    /// �]�w����������Ĳ�o���\��
    /// </summary>
    /// <param name="action">�\��</param>
    /// <param name="val">�ˮ`�ƭ�</param>
    public void SetAttackAction(Action<float> action, float val)
    {
        attackAction = action;
        attackPower = val;
    }
    /// <summary>
    /// �]�w����������Ĳ�o���o�g���楴���\��
    /// </summary>
    /// <param name="action"></param>
    public void SetAttackAction(Action action)
    {
        skillAction = action;
    }

    /// <summary>
    /// ��������(�ʵe)�ƥ�
    /// </summary>
    public void AttackDone()
    {
         if(attackAction != null) attackAction(attackPower);
         attackAction = null;
         if (skillAction != null) skillAction();
         skillAction = null;
    }

}
