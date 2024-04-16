using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterCtrl : MonoBehaviour
{
    #region 基本元件
    /// <summary>
    /// 腳色控制器
    /// </summary>
    private CharacterController _charCtrl;
    private CharacterController charCtrl
    {
        get
        {
            if (_charCtrl == null)
                _charCtrl = GetComponent<CharacterController>();
            return _charCtrl;
        }
    }
    /// <summary>
    /// 動畫控制器
    /// </summary>
    private AnimationCtrl _aniCtrl;
    private AnimationCtrl aniCtrl
    {
        get
        {
            if (_aniCtrl == null)
                _aniCtrl = GetComponentInChildren<AnimationCtrl>();
            return _aniCtrl;
        }
    }
    #endregion 基本元件

    #region 路徑導引
    /// <summary>
    /// 路徑節點序號
    /// </summary>
    private int pathIndex;
    /// <summary>
    /// 路徑控制
    /// </summary>
    private PathCtrl path;
    /// <summary>
    /// 移動節點目標
    /// </summary>
    private Transform moveTarget;
    #endregion 路徑導引

    /// <summary>
    /// 攻擊目標
    /// </summary>
    private CharacterCtrl attackTarget;
    /// <summary>
    /// 所屬陣營
    /// </summary>
    private PlayerGroup group;
    /// <summary>
    /// 取得陣營(唯讀)
    /// </summary>
    public PlayerGroup getGroup
    {
        get { return group; }
    }
    /// <summary>
    /// 腳色狀態
    /// </summary>
    private AniType state
    {
        get
        {
            if (!isAttacking && motion.z != 0)
            {// 移動狀態: 不再攻擊中 && 前進動能不為零
                return AniType.Move;
            }
            else return AniType.Idle;
        }
    }
    private bool isAttacking 
    {
        get 
        {
            return attackTarget != null && 
                this.InRange(attackTarget, attackRange);
        }
    }

    /// <summary>
    /// 是否有目標? 前進:不動
    /// </summary>
    private float movement
    {
        get
        {//(if()的簡寫:布林 ? true :  false ;)
            return moveTarget != null || attackTarget != null ?
                  moveSpeed * Time.deltaTime : 0;
        }
    }

    /// <summary>
    /// 腳色動能
    /// </summary>
    private Vector3 _motion;
    public Vector3 motion
    {
        get
        {
            return transform.forward * movement;
        }
    }
    /// <summary>
    /// 角色正在進行的行為動作
    /// Action用來裝沒有回傳值，無參數的功能方法
    /// </summary>
    private Action actionMode;
    #region 基本參數
    [Header("單位類型")]
    public UnitType unitType;
    [Header("攻擊類型")]
    public AttackType attackType;
    public FlySkill flySkill;
    [Header("血量")]
    public float hpMax = 10;//HP分母
    private float hp;//HP分子(當前血量)
    /// <summary>
    /// UI用的百分點數據
    /// </summary>
    public float hpFillamount
    {
        get 
        {
            return hp / hpMax;
        }
    }
    [Header("移動速度")]
    [Range(0.0f, 2.0f)]
    public float moveSpeed = 1f;
    [Header("搜索範圍")]
    public float searchRange = 3f;
    [Header("攻擊距離")]
    public float attackRange = 1.5f;
    [Header("攻擊速度(間隔秒)")]
    public float attackTime = 1.5f;
    /// <summary>
    /// 攻擊冷卻計時器 (倒數至0即完成)
    /// </summary>
    private float attackTimer = 0f;
    /// <summary>
    /// 攻擊是否已冷卻
    /// </summary>
    private bool attackCD
    {
        get 
        {
            return attackTimer <= 0f;
        }
    }
    [Header("攻擊力")]
    public float attackPower = 10f;
    [Header("回收時間")]
    public float recoveryTime = 1.5f;
    private float recoveryTimer = 0f;
    private bool recoveryCD
    {
        get
        {
            return recoveryTimer <= 0f;
        }
    }
    /// <summary>
    /// 角色是否死亡
    /// </summary>
    public bool isDead
    {
        get 
        {
            return hp <= 0;
        }
    }

    public bool IsDead()
    {
        return hp <= 0;
    }


    [Header("擊中音效")]
    public AudioClip hitSoundEffect;
    #endregion 基本參數
    // 初始化
    void Start()
    {//起始動作行為:移動模式
        actionMode = MoveMode;
        //回滿血HP登場
        hp = hpMax;
        //特殊單位設定
        switch (unitType)
        {
            case UnitType.Boss:
                //產生定位點(即時)
                moveTarget = new GameObject("BossHome").transform;
                //將定位點放置腳下
                moveTarget.transform.position = transform.position;
                //設定陣營
                SetGroup(PlayerGroup.P2);
                //系統通報
                GameSystem.SetBossCtrl(this);
                break;

            case UnitType.Base:
                //設定陣營
                SetGroup(PlayerGroup.P1);
                //系統通報
                GameSystem.SetBaseCtrl(this);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameSystem.inBattle) return;
        //執行當前的行為模式
        //MoveMode 或 AttackMode
        actionMode();
        aniCtrl.SetAniType(state);
    }

    /// <summary>
    /// 設定陣營
    /// </summary>
    /// <param name="playerGroup">角色陣營</param>
    public CharacterCtrl SetGroup(PlayerGroup playerGroup)
    {
        group = playerGroup;
        //加入目標清單
        group.AddTarget(this);
        return this;
    }

    [ContextMenu("設為P1")]
    public void SetGroupP1()
    {
        group.RemoveTarget(this);
        group = PlayerGroup.P1;
        //加入目標清單
        group.AddTarget(this);
    }
    [ContextMenu("設為P2")]
    public void SetGroupP2()
    {
        group.RemoveTarget(this);
        group = PlayerGroup.P2;
        //加入目標清單
        group.AddTarget(this);
    }

    /// <summary>
    /// 設定路徑
    /// </summary>
    /// <param name="pathCtrl">路徑控制器</param>
    public void SetPath(PathCtrl pathCtrl)
    {
        //依照陣營轉換 : 從頭(0)或尾(last)開始
        pathIndex = group == PlayerGroup.P1 ? 0 : pathCtrl.lastIndex;
        //記住路徑控制器
        path = pathCtrl;
        //首個移動目標取得
        moveTarget = path.GetPoint(pathIndex);
    }

    /// <summary>
    /// 移動模式
    /// </summary>
    void MoveMode()
    {
        Debug.Log("移動模式");
        if (attackTarget && !attackTarget.isDead)
        {//找到目標:切換成攻擊模式
            actionMode = AttackMode;
        }
        else 
        {
            //掃描周遭敵方目標
            attackTarget = this.SearchTarget(searchRange);
        }

        if (moveTarget)
        {

            //靠近至路徑點半徑0.5內切換至下一個路徑點
            if (transform.InRange(moveTarget, 0.5f))
            {
                if (unitType == UnitType.None)
                {
                    //跳至下一號
                    /*if(group == PlayerGroup.P1) pathIndex++;
                    else pathIndex--;*/
                    pathIndex += group == PlayerGroup.P1 ? 1 : -1;
                    //取得下一個點物件
                    moveTarget = path.GetPoint(pathIndex);
                }
                if (unitType == UnitType.Boss)
                {
                    //可選擇隨機旋轉(閒逛)
                    //可選擇移除目標(原地待著)
                    //moveTarget = null;
                }
            }
            else
            {
                //朝向路徑點方位鎖定
                transform.LookAt(moveTarget);
            }
        }
        //前進依照motion在前進
        charCtrl.Move(motion);
    }

    /// <summary>
    /// 敵方進入搜索範圍 : 切換攻擊模式
    /// </summary>
    void AttackMode()
    {
       
        Debug.Log("攻擊模式");
        if (attackTarget && !attackTarget.isDead)
        {//目標還在:執行追擊/攻擊 Debug.Log("追擊/追擊");
            //朝向敵人方位鎖定
            transform.LookAt(attackTarget.transform);
            if (this.InRange(attackTarget, attackRange))
            {
                if (attackCD)
                {//攻擊冷卻 : Debug.Log("攻擊");                  
                    attackTimer = attackTime;
                    //播放攻擊動畫，造成目標損傷
                    if (attackType == AttackType.Melee)
                    {//近戰攻擊 : 直接造成傷害
                        aniCtrl.SetAttackAction(attackTarget.HpCtrl, -attackPower);
                    }
                    else
                    {
                        aniCtrl.SetAttackAction(ShootSkill);
                    }                       
                    aniCtrl.SetAttackSpeed(attackTime);
                    aniCtrl.SetTrigger(AniType.Attack);                  
                }
                else
                {//倒數計時
                    attackTimer -= Time.deltaTime;
                }
            }
            else
            {//Debug.Log("追擊");
                charCtrl.Move(motion);
            }
        }
        else
        {//目標消失:
            actionMode = MoveMode;
        }

    }

    public void ShootSkill()
    {
        if (flySkill)
        {
            Instantiate(flySkill, transform.position, transform.rotation)
            .SetTarget(attackTarget, attackPower);

        }
    }

        /// <summary>
        /// 血量控制
        /// </summary>
        /// <param name="val"></param>
      public void HpCtrl(float val)
      {
        if (isDead) return;      
        //hp += val;
        hp = Mathf.Clamp(hp += val, 0, hpMax);
        SoundManager.instance.PlaySFX(hitSoundEffect);
        //Debug.Log($"HP:{hp}/{hpMax}");
        //通知系統 : HP變化需要更新(單位)
        GameSystem.UpdateHpBar(unitType);
        if (isDead)
        {
            Dead();
        }

      }

    /// <summary>
    /// 死亡相關邏輯
    /// </summary>
    void Dead()
    {//角色死亡
        //Debug.Log("角色死亡 ");
        //觸發死亡動畫
        aniCtrl.SetTrigger(AniType.Dead);
        //移出目標系統
        group.RemoveTarget(this);
        //回收倒數開始
        recoveryTimer = recoveryTime;
        //切換至回收狀態
        actionMode = Recovery;       
        //關閉CharCtrl
        charCtrl.enabled = false;
    }
    /// <summary>
    /// 回收機制
    /// </summary>
    void Recovery()
    {
        //Debug.Log($"角色等待回收:{recoveryTimer} ");
        recoveryTimer -= Time.deltaTime;
        if (recoveryCD)
        {//物件池:物件回收再利用
            Destroy(gameObject);//摧毀物件
        }
    }

    /// <summary>
    /// 依照本控制器 Move 執行方向觸發
    /// </summary>
    /// <param name="hit">碰到對象</param>
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        CharacterCtrl ctrl = this.PushTarget(hit.gameObject);
        //如果未找到腳色控制器不做任何事
        if(ctrl == null) return;
        //推擠他人
        ctrl.PushMove(hit.moveDirection);
    }
    /// <summary>
    /// 推動(外力)
    /// </summary>
    public void PushMove(Vector3 pushDir)
    {
        charCtrl.Move(pushDir * Time.deltaTime * 2);
    }
}
