using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationCtrl : MonoBehaviour
{
    #region 基本元件
    /// <summary>
    /// Animator 物件實體
    /// </summary>
    private Animator _animator;
    /// <summary>
    /// 對外對內呼叫用公開接口(唯讀)
    /// </summary>
    public Animator animator
    {
        get 
        {
            if (_animator == null)//第一次呼叫時抓取
                _animator = GetComponent<Animator>();
            return _animator;
        }
    }

    /// <summary>
    /// 攻擊動作檔片段
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
                {//遍歷(集合物件掃描)
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
    #endregion 基本元件

    /// <summary>
    /// 攻擊事件(委託)
    /// </summary>
    private Action<float> attackAction;
    private Action skillAction;
    private float attackPower;


    /// <summary>
    /// 設定Animation Type動畫狀態機
    /// </summary>
    /// <param name="type">動畫類型(狀態)</param>
    public void SetAniType(AniType type)
    {
        //切換於 Idle 和 Move 之間
        animator.SetBool("Move", type == AniType.Move);
    }

    /// <summary>
    /// 觸發型動畫(一次性播放)
    /// </summary>
    /// <param name="type">動畫類型</param>
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
    /// 設定攻擊完成時觸發的功能
    /// </summary>
    /// <param name="action">功能</param>
    /// <param name="val">傷害數值</param>
    public void SetAttackAction(Action<float> action, float val)
    {
        attackAction = action;
        attackPower = val;
    }
    /// <summary>
    /// 設定攻擊完成時觸發的發射飛行打擊功能
    /// </summary>
    /// <param name="action"></param>
    public void SetAttackAction(Action action)
    {
        skillAction = action;
    }

    /// <summary>
    /// 攻擊完成(動畫)事件
    /// </summary>
    public void AttackDone()
    {
         if(attackAction != null) attackAction(attackPower);
         attackAction = null;
         if (skillAction != null) skillAction();
         skillAction = null;
    }

}
