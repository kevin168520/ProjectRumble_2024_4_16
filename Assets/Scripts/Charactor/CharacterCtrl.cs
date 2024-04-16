using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterCtrl : MonoBehaviour
{
    #region �򥻤���
    /// <summary>
    /// �}�ⱱ�
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
    /// �ʵe���
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
    #endregion �򥻤���

    #region ���|�ɤ�
    /// <summary>
    /// ���|�`�I�Ǹ�
    /// </summary>
    private int pathIndex;
    /// <summary>
    /// ���|����
    /// </summary>
    private PathCtrl path;
    /// <summary>
    /// ���ʸ`�I�ؼ�
    /// </summary>
    private Transform moveTarget;
    #endregion ���|�ɤ�

    /// <summary>
    /// �����ؼ�
    /// </summary>
    private CharacterCtrl attackTarget;
    /// <summary>
    /// ���ݰ}��
    /// </summary>
    private PlayerGroup group;
    /// <summary>
    /// ���o�}��(��Ū)
    /// </summary>
    public PlayerGroup getGroup
    {
        get { return group; }
    }
    /// <summary>
    /// �}�⪬�A
    /// </summary>
    private AniType state
    {
        get
        {
            if (!isAttacking && motion.z != 0)
            {// ���ʪ��A: ���A������ && �e�i�ʯण���s
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
    /// �O�_���ؼ�? �e�i:����
    /// </summary>
    private float movement
    {
        get
        {//(if()��²�g:���L ? true :  false ;)
            return moveTarget != null || attackTarget != null ?
                  moveSpeed * Time.deltaTime : 0;
        }
    }

    /// <summary>
    /// �}��ʯ�
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
    /// ���⥿�b�i�檺�欰�ʧ@
    /// Action�ΨӸ˨S���^�ǭȡA�L�Ѽƪ��\���k
    /// </summary>
    private Action actionMode;
    #region �򥻰Ѽ�
    [Header("�������")]
    public UnitType unitType;
    [Header("��������")]
    public AttackType attackType;
    public FlySkill flySkill;
    [Header("��q")]
    public float hpMax = 10;//HP����
    private float hp;//HP���l(��e��q)
    /// <summary>
    /// UI�Ϊ��ʤ��I�ƾ�
    /// </summary>
    public float hpFillamount
    {
        get 
        {
            return hp / hpMax;
        }
    }
    [Header("���ʳt��")]
    [Range(0.0f, 2.0f)]
    public float moveSpeed = 1f;
    [Header("�j���d��")]
    public float searchRange = 3f;
    [Header("�����Z��")]
    public float attackRange = 1.5f;
    [Header("�����t��(���j��)")]
    public float attackTime = 1.5f;
    /// <summary>
    /// �����N�o�p�ɾ� (�˼Ʀ�0�Y����)
    /// </summary>
    private float attackTimer = 0f;
    /// <summary>
    /// �����O�_�w�N�o
    /// </summary>
    private bool attackCD
    {
        get 
        {
            return attackTimer <= 0f;
        }
    }
    [Header("�����O")]
    public float attackPower = 10f;
    [Header("�^���ɶ�")]
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
    /// ����O�_���`
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


    [Header("��������")]
    public AudioClip hitSoundEffect;
    #endregion �򥻰Ѽ�
    // ��l��
    void Start()
    {//�_�l�ʧ@�欰:���ʼҦ�
        actionMode = MoveMode;
        //�^����HP�n��
        hp = hpMax;
        //�S����]�w
        switch (unitType)
        {
            case UnitType.Boss:
                //���ͩw���I(�Y��)
                moveTarget = new GameObject("BossHome").transform;
                //�N�w���I��m�}�U
                moveTarget.transform.position = transform.position;
                //�]�w�}��
                SetGroup(PlayerGroup.P2);
                //�t�γq��
                GameSystem.SetBossCtrl(this);
                break;

            case UnitType.Base:
                //�]�w�}��
                SetGroup(PlayerGroup.P1);
                //�t�γq��
                GameSystem.SetBaseCtrl(this);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameSystem.inBattle) return;
        //�����e���欰�Ҧ�
        //MoveMode �� AttackMode
        actionMode();
        aniCtrl.SetAniType(state);
    }

    /// <summary>
    /// �]�w�}��
    /// </summary>
    /// <param name="playerGroup">����}��</param>
    public CharacterCtrl SetGroup(PlayerGroup playerGroup)
    {
        group = playerGroup;
        //�[�J�ؼвM��
        group.AddTarget(this);
        return this;
    }

    [ContextMenu("�]��P1")]
    public void SetGroupP1()
    {
        group.RemoveTarget(this);
        group = PlayerGroup.P1;
        //�[�J�ؼвM��
        group.AddTarget(this);
    }
    [ContextMenu("�]��P2")]
    public void SetGroupP2()
    {
        group.RemoveTarget(this);
        group = PlayerGroup.P2;
        //�[�J�ؼвM��
        group.AddTarget(this);
    }

    /// <summary>
    /// �]�w���|
    /// </summary>
    /// <param name="pathCtrl">���|���</param>
    public void SetPath(PathCtrl pathCtrl)
    {
        //�̷Ӱ}���ഫ : �q�Y(0)�Χ�(last)�}�l
        pathIndex = group == PlayerGroup.P1 ? 0 : pathCtrl.lastIndex;
        //�O����|���
        path = pathCtrl;
        //���Ӳ��ʥؼШ��o
        moveTarget = path.GetPoint(pathIndex);
    }

    /// <summary>
    /// ���ʼҦ�
    /// </summary>
    void MoveMode()
    {
        Debug.Log("���ʼҦ�");
        if (attackTarget && !attackTarget.isDead)
        {//���ؼ�:�����������Ҧ�
            actionMode = AttackMode;
        }
        else 
        {
            //���y�P�D�Ĥ�ؼ�
            attackTarget = this.SearchTarget(searchRange);
        }

        if (moveTarget)
        {

            //�a��ܸ��|�I�b�|0.5�������ܤU�@�Ӹ��|�I
            if (transform.InRange(moveTarget, 0.5f))
            {
                if (unitType == UnitType.None)
                {
                    //���ܤU�@��
                    /*if(group == PlayerGroup.P1) pathIndex++;
                    else pathIndex--;*/
                    pathIndex += group == PlayerGroup.P1 ? 1 : -1;
                    //���o�U�@���I����
                    moveTarget = path.GetPoint(pathIndex);
                }
                if (unitType == UnitType.Boss)
                {
                    //�i����H������(���})
                    //�i��ܲ����ؼ�(��a�ݵ�)
                    //moveTarget = null;
                }
            }
            else
            {
                //�¦V���|�I�����w
                transform.LookAt(moveTarget);
            }
        }
        //�e�i�̷�motion�b�e�i
        charCtrl.Move(motion);
    }

    /// <summary>
    /// �Ĥ�i�J�j���d�� : ���������Ҧ�
    /// </summary>
    void AttackMode()
    {
       
        Debug.Log("�����Ҧ�");
        if (attackTarget && !attackTarget.isDead)
        {//�ؼ��٦b:����l��/���� Debug.Log("�l��/�l��");
            //�¦V�ĤH�����w
            transform.LookAt(attackTarget.transform);
            if (this.InRange(attackTarget, attackRange))
            {
                if (attackCD)
                {//�����N�o : Debug.Log("����");                  
                    attackTimer = attackTime;
                    //��������ʵe�A�y���ؼзl��
                    if (attackType == AttackType.Melee)
                    {//��ԧ��� : �����y���ˮ`
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
                {//�˼ƭp��
                    attackTimer -= Time.deltaTime;
                }
            }
            else
            {//Debug.Log("�l��");
                charCtrl.Move(motion);
            }
        }
        else
        {//�ؼЮ���:
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
        /// ��q����
        /// </summary>
        /// <param name="val"></param>
      public void HpCtrl(float val)
      {
        if (isDead) return;      
        //hp += val;
        hp = Mathf.Clamp(hp += val, 0, hpMax);
        SoundManager.instance.PlaySFX(hitSoundEffect);
        //Debug.Log($"HP:{hp}/{hpMax}");
        //�q���t�� : HP�ܤƻݭn��s(���)
        GameSystem.UpdateHpBar(unitType);
        if (isDead)
        {
            Dead();
        }

      }

    /// <summary>
    /// ���`�����޿�
    /// </summary>
    void Dead()
    {//���⦺�`
        //Debug.Log("���⦺�` ");
        //Ĳ�o���`�ʵe
        aniCtrl.SetTrigger(AniType.Dead);
        //���X�ؼШt��
        group.RemoveTarget(this);
        //�^���˼ƶ}�l
        recoveryTimer = recoveryTime;
        //�����ܦ^�����A
        actionMode = Recovery;       
        //����CharCtrl
        charCtrl.enabled = false;
    }
    /// <summary>
    /// �^������
    /// </summary>
    void Recovery()
    {
        //Debug.Log($"���ⵥ�ݦ^��:{recoveryTimer} ");
        recoveryTimer -= Time.deltaTime;
        if (recoveryCD)
        {//�����:����^���A�Q��
            Destroy(gameObject);//�R������
        }
    }

    /// <summary>
    /// �̷ӥ���� Move �����VĲ�o
    /// </summary>
    /// <param name="hit">�I���H</param>
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        CharacterCtrl ctrl = this.PushTarget(hit.gameObject);
        //�p�G�����}�ⱱ����������
        if(ctrl == null) return;
        //�����L�H
        ctrl.PushMove(hit.moveDirection);
    }
    /// <summary>
    /// ����(�~�O)
    /// </summary>
    public void PushMove(Vector3 pushDir)
    {
        charCtrl.Move(pushDir * Time.deltaTime * 2);
    }
}
