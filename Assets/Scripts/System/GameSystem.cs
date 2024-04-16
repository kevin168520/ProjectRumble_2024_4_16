using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �I���B��G�R�A��ƺ޲z����
/// </summary>
public static class GameSystem
{
    #region PlayerInfo
    /// <summary>
    /// ��e����
    /// </summary>
    private static int _level = 1;
    public static int level 
    {
        get
        {
            return _level;
        }     
    }
    /// <summary>
    /// ���o���`�g��
    /// </summary>
    public static int expTotal { get; private set; }
    /// <summary>
    /// ��ܥΪ��g����l
    /// </summary>
    public static int exp 
    { 
        get
        {
            int expBefore = 0;
            for (int i = 0; i < _level; i++)
            {
                expBefore += i * 10;
            }

            return expTotal  - expBefore;
        }
    }
    /// <summary>
    /// ��ܥΪ��g�����
    /// </summary>
    public static int expMax 
    {
        get
        {
            return _level * 10;
        }
    }
    /// <summary>
    /// UI�θg��ʤ��I
    /// </summary>
    public static float expFillAmount
    {
        get
        {
            return (float)exp / (float)expMax; 
        }
    }
    /// <summary>
    /// ����g���
    /// </summary>
    /// <param name="val">�g��ƾ�</param>
    public static void ExpCtrl(int val)
    {
        expTotal += val;
        if (exp >= expMax)
        {//�g����l�j�󵥩���� : �ɯ�
            _level++;
        }
    }

    /// <summary>
    /// ���d���{��r��
    /// </summary>
    public static string lvMileageStr
    {
        get
        {//��ܮɴ� 1 ��� : �w�F��������
            return (lvMileage - 1).ToString();
        }
    }

    /// <summary>
    /// ��������
    /// </summary>
    public static int money { get; private set; }
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="val">����ƾ�</param>
    public static void MoneyCtrl(int val)
    {
        //������(�tvalĲ�o)
        if (money + val < 0) return;
        money += val;
    }
    #endregion PlayerInfo

    #region StageManager
    /// <summary>
    /// ���d���{(�i�� : ��e�Ʀr�O���b�𲤪����d)
    /// </summary>
    public static int lvMileage = 1;
    /// <summary>
    /// ��e�R�x����
    /// </summary>
    private static int currentStageIndex
    {
        get
        {//���d���{ �� 5 + 1
            return lvMileage / 5 + 1;
        }
    }
    /// <summary>
    /// ��e���d����
    /// </summary>
    private static int currentLevelIndex
    {
        get
        {//���d���{�� 5 ���l��
            return lvMileage % 5;
        }
    }
    /// <summary>
    /// ��ӮɩI�s�ˬd
    /// </summary>
    public static void CheckLvMileage()
    {//��������{�O�_���i�{
        if (selectLvMileage >= lvMileage) lvMileage++;
    }
    /// <summary>
    /// ���ê������m
    /// </summary>
    private static StageManager _stageManager;
    /// <summary>
    /// ��~���}���f(��Ū)
    /// </summary>
    public static StageManager stageManager
    {
        get { return _stageManager; }
    }
    public static void SetStageManager(StageManager manager)
    {
        _stageManager = manager;
        _stageManager.SetGPS(currentStageIndex, currentLevelIndex);
    }
    #endregion StageManager

    #region UIStageMenu
    /// <summary>
    /// ���}�Ҥ��� Stage �s��
    /// </summary>
    private static int selectStageIndex;
    /// <summary>
    /// �I��}�Ҥ� Stage �� Level
    /// </summary>
    private static int selectLevelIndex;
    public static int selectLvMileage
    {
        get
        {
            return (selectStageIndex - 1) * 5 + selectLevelIndex + 1;
        }
    }

    /// <summary>
    /// stageMenuCtrl ���鱱��}��
    /// </summary>
    private static UIStageMenuCtrl stageMenuCtrl;
    /// <summary>
    /// stageMenuCtrl �]�w�\��
    /// </summary>
    /// <param name="ctrl">���</param>
    public static void SetStageMenuCtrl(UIStageMenuCtrl ctrl)
    {
        stageMenuCtrl = ctrl;
    }
    /// <summary>
    /// levelInfoCtrl ���鱱��}��
    /// </summary>
    private static UILevelInfoCtrl levelInfoCtrl;
    /// <summary>
    /// levelInfoCtrl �]�w�\��
    /// </summary>
    /// <param name="ctrl">���</param>
    public static void SetLevelInfoCtrl(UILevelInfoCtrl ctrl)
    {
        levelInfoCtrl = ctrl;
    }

    /// <summary>
    /// ��s StageUI ���e�A�åB�O���}�Ҫ� StageIndex
    /// </summary>
    /// <param name="stageIndex">���d�s��</param>
    public static void UpdateStageUI(int stageIndex)
    {
        selectStageIndex = stageIndex;
        stageMenuCtrl.UpdateMenu(stageManager.GetStageDB(selectStageIndex).levels);
        stageMenuCtrl.UpdateHeader($"�a�� {selectStageIndex}");
    }

    /// <summary>
    /// ��w Level 
    /// </summary>
    /// <param name="levelIndex">Level ���X</param>
    public static void SelectLevel(int levelIndex)
    {
        selectLevelIndex = levelIndex;
        //�}�� Level ���O(��� Level Info)
        levelInfoCtrl.UpdateInfo(stageManager.
            GetStageDB(selectStageIndex).levels[selectLevelIndex]);
    }

    public static void LevelStart()
    {
        ToSceneByName($"Stage{selectStageIndex}-{selectLevelIndex}");
    }
    #endregion UIStageMenu

    #region ���������޲z
    /// <summary>
    /// ��e�Ҧb�����W��(���d�W)
    /// </summary>
    public static string currentSceneName { get; private set; }
    /// <summary>
    /// �H�����W�٤���(���d�W)
    /// </summary>
    /// <param name="sceneName">�����W��</param>
    public static void ToSceneByName(string sceneName)
    {
        currentSceneName = sceneName;
        //�����޲z�H.Ū������(�����W��);
        SceneManager.LoadScene(sceneName);
    }
    /// <summary>
    /// ������e�ϥγ���
    /// </summary>
    public static void ReloadScene()
    {
        SceneManager.LoadScene(currentSceneName);
    }
    /// <summary>
    /// �MStage�j�w�|�[
    /// </summary>
    public static void AddGameUI()
    {
        SceneManager.LoadScene("GameUI", LoadSceneMode.Additive);
    }
    #endregion

    #region �ؼШt��
    /// <summary>
    /// P1�b���W������M��
    /// </summary>
    private static List<CharacterCtrl> listP1 = new List<CharacterCtrl>();
    /// <summary>
    /// P2�b���W������M��
    /// </summary>
    private static List<CharacterCtrl> listP2 = new List<CharacterCtrl>();
    /// <summary>
    /// �N�ؼХ[�J�޲z�t��
    /// </summary>
    /// <param name="group">�}�����</param>
    /// <param name="character">�ؼШ���</param>
    public static void AddTarget(this PlayerGroup group, CharacterCtrl character)
    {
        switch (group)
        {//�̷Ӥ��ըM�w�[�J���M��
            case PlayerGroup.P1:
                listP1.Add(character);
                break;

            case PlayerGroup.P2:
                listP2.Add(character);
                break;
        }
    }
    /// <summary>
    /// �N�ؼв����޲z�t��
    /// </summary>
    /// <param name="group">�}�����</param>
    /// <param name="character">�ؼШ���</param>
    public static void RemoveTarget(this PlayerGroup group, CharacterCtrl character)
    {
        switch (group)
        {//�̷Ӥ��ըM�w�[�J���M��
            case PlayerGroup.P1:
                listP1.Remove(character);
                break;

            case PlayerGroup.P2:
                listP2.Remove(character);
                break;
        }
    }

    /// <summary>
    /// ���o�Ĥ�ؼвM��
    /// </summary>
    /// <param name="main">�ШD��</param>
    /// <returns>�Ĥ�ؼвM��</returns>
    public static List<CharacterCtrl> GetEnemyList(this CharacterCtrl main)
    {
        return main.getGroup == PlayerGroup.P1 ? listP2 : listP1;
    }
    /// <summary>
    /// ���o�ڤ�ؼвM��
    /// </summary>
    /// <param name="main">�ШD��</param>
    /// <returns>�ڤ�ؼвM��</returns>
    public static List<CharacterCtrl> GetTeamList(this CharacterCtrl main)
    {
        return main.getGroup == PlayerGroup.P2 ? listP2 : listP1;
    }
    /// <summary>
    /// �M���ؼШt�Τ����ª���
    /// </summary>
    public static void ClearTargetSystem()
    {
        listP1.Clear(); listP2.Clear();
    }
    /// <summary>
    /// ���o�Ĥ�ؼ�(�Z���̪�)
    /// </summary>
    /// <param name="main">�ШD��</param>
    /// <param name="range">�d��</param>
    /// <returns>�Ĥ�ؼ�</returns>
    public static CharacterCtrl SearchTarget(this CharacterCtrl main, float range)
    {
        float minRange = 999f;//�w�]�Z��
        CharacterCtrl target = null;
        CharacterCtrl closestTarget = null;
        for (int i = 0; i < main.GetEnemyList().Count; i++)
        {//�M�d�M�椺����H�G�O�_�b�d��
            target = main.GetEnemyList()[i];
            //�b�j���d�� && ��̫�d�̪߳�ؼжZ���٪�
            if (main.InRange(target, range) && main.GetDistance(target) < minRange)
            {
                //��s�̪�ؼ�
                closestTarget = target;
                //��s�̪�Z��
                minRange = main.GetDistance(target);
            }
        }
        return closestTarget;
    }
    /// <summary>
    /// �����ڤ���
    /// </summary>
    /// <param name="main">�ШD��</param>
    /// <param name="hitObj"></param>
    /// <returns></returns>
    public static CharacterCtrl PushTarget(this CharacterCtrl main, GameObject hitObj)
    {
       
        CharacterCtrl target = null;
        CharacterCtrl pushTarget = null;
        for (int i = 0; i < main.GetTeamList().Count; i++)
        {//�M�d�M�椺����H�G�O�_�b�d��
            target = main.GetTeamList()[i];
            //�b�j���d�� && ��̫�d�̪߳�ؼжZ���٪�
            if (hitObj == target.gameObject)
            {
                //�����ؼ�
                pushTarget = target;
                break;
            }
        }
        return pushTarget;
    }
    #endregion �ؼШt��

    #region �X�R(�u��)�\��

    public static float GetDistance(this CharacterCtrl center, CharacterCtrl target)
    {
        return Vector3.Distance(center.transform.position, target.transform.position);
    }
    /// <summary>
    /// �ˬd�ؼЪ���O�_�b���ߪ����w�d��
    /// </summary>
    /// <param name="center">���ߪ���(�X�R��H)</param>
    /// <param name="target">�ؼЪ���(�ˬd��H)</param>
    /// <param name="range">�ˬd�d��</param>
    /// <returns>�O/�_�b�d��</returns>
    public static bool InRange(this Transform center, Transform target, float range)
    {
        return Vector3.Distance(center.position, target.position) <= range;
    }
    /// <summary>
    /// �ˬd�ؼЪ���O�_�b���ߪ����w�d��
    /// </summary>
    /// <param name="center">���ߪ���(�X�R��H)</param>
    /// <param name="target">�ؼЪ���(�ˬd��H)</param>
    /// <param name="range">�ˬd�d��</param>
    /// <returns>�O/�_�b�d��</returns>
    public static bool InRange(this CharacterCtrl center, CharacterCtrl target, float range)
    {
        return Vector3.Distance(center.transform.position, target.transform.position) <= range;
    }
    #endregion �X�R(�u��)�\��

    #region �԰���T�t��
    
    public static bool inBattle { get; private set; }
    public static void StartBattle()
    {
        //�}��
        inBattle = true;
        //�}�l�ͩ�
        spawnerManager.StartSpawn();
    }
    public static SpawnerManager spawnerManager { get; private set; }
    public static void SetSpawnerManager(SpawnerManager spawner)
    {
        //Spawner�U��
        spawnerManager = spawner;
       
        //��l��q
        _energy = 7f;
    }


    /// <summary>
    /// �԰��p��(�����Ʀs��)
    /// </summary>
    private static int _timer = 210;
    /// <summary>
    /// �԰��p��(�ާ@���α��f)
    /// </summary>
    public static int timer
    {
        get 
        {
            return _timer;
        }
        set 
        {
            _timer = Mathf.Clamp(value, 0, 210);
        }
    }
    /// <summary>
    /// �ӧQ�P�w:�p�ɾ��j��0 & Boss ���`
    /// </summary>
    public static bool win
    {
        get 
        {
            return timer > 0 && p1Win;
        }
    }

    /// <summary>
    /// �ڤ�Base���ⱱ�
    /// </summary>
    public static CharacterCtrl baseCtrl { get; private set; }
    public static bool p1Win
    {
        get 
        {
            return bossCtrl.isDead;
        }
    }

    /// <summary>
    /// �Ĥ�Boss���ⱱ�
    /// </summary>
    public static CharacterCtrl bossCtrl { get; private set; }
    public static bool p2Win
    {
        get
        {
            return baseCtrl.isDead;
        }
    }



    /// <summary>
    /// �]�w Base ���ⱱ�
    /// </summary>
    /// <param name="ctrl"></param>
    public static void SetBaseCtrl(CharacterCtrl ctrl)
    {
        baseCtrl = ctrl;
        upPanelCtrl?.HpBarCtrlP1(baseCtrl.hpFillamount);
    }

    /// <summary>
    /// �]�w Boss ���ⱱ�
    /// </summary>
    /// <param name="ctrl"></param>
    public static void SetBossCtrl(CharacterCtrl ctrl)
    {
        bossCtrl = ctrl;
        upPanelCtrl?.HpBarCtrlP2(bossCtrl.hpFillamount);
    }
    /// <summary>
    /// ���w��s�� HUD UI
    /// </summary>
    /// <param name="type">���w���������</param>
    public static void UpdateHpBar(UnitType type)
    {
        if(type == UnitType.Base)
            upPanelCtrl?.HpBarCtrlP1(baseCtrl.hpFillamount);
        if (type == UnitType.Boss)
        {
            upPanelCtrl?.HpBarCtrlP2(bossCtrl.hpFillamount);
            if (bossCtrl.isDead)
            {
               CheckLvMileage();
               ExpCtrl(20);
               MoneyCtrl(30);
            }
        }
           
    }

    /// <summary>
    /// �W�譱�OUI����
    /// </summary>
    public static UIUpPanelCtrl upPanelCtrl { get; private set; }

    /// <summary>
    /// �]�w�W�譱�OUI����(�U��)
    /// </summary>
    /// <param name="ctrl">�W�譱�OUI����</param>
    public static void SetUpPanelCtrl(UIUpPanelCtrl ctrl)
    {
        upPanelCtrl = ctrl;
        //����� Boss/Base �z�L�U��UI��s HP �ʤ��I
        if (baseCtrl) upPanelCtrl.HpBarCtrlP1(baseCtrl.hpFillamount);
        if (bossCtrl) upPanelCtrl.HpBarCtrlP2(bossCtrl.hpFillamount);
        //���s�p�ɾ�
        _timer = 210;
        //�԰��w��
        inBattle = false;
    }

    /// <summary>
    /// ��q��(�����Ʀs��)
    /// </summary>
    private static float _energy = 0f;
    /// <summary>
    /// ��q��(�ާ@���α��f)
    /// </summary>
    public static float energy
    {
        get 
        {
            return _energy;
        }
        set 
        {
            _energy = Mathf.Clamp(value, 0f, 10f);
        }
    }
    /// <summary>
    /// �԰���T���OUI����
    /// </summary>
    public static UIBattlePanelCtrl battlePanelUI { get; private set; }
    /// <summary>
    /// �԰���T���OUI����(�U��)
    /// </summary>
    /// <param name="ctrl">�԰���T���OUI����</param>
    public static void SetBattlePanelUI(UIBattlePanelCtrl ctrl)
    {
        battlePanelUI = ctrl;
    }
    /// <summary>
    /// �q�����D�諸�԰����(�Ȧs)
    /// </summary>
    public static UIRumbleInfoCtrl selectRumbleUI { get; private set; }
    
    public static CharacterCtrl selectRumble
    {
        get 
        {
            return selectRumbleUI.data.characterCtrl;
        }
    }
    /// <summary>
    /// �O�_����԰����
    /// </summary>
    public static bool isSelectRumble
    {
        get 
        {
           /* selectRumbleUI?.NoData();

            if (selectRumbleUI)
            {
                selectRumbleUI.NoData();
            }*/           
            return selectRumbleUI != null && 
                !selectRumbleUI.noData;
        }
    }
    public static Action<bool> dropZoneShowUp { get; private set; }
    public static void SetShowUp(Action<bool> action)
    {
        dropZoneShowUp += action;
    }
    public static void RemoveShowUp(Action<bool> action)
    {
        dropZoneShowUp -= action;
    }

    /// <summary>
    /// �����]�w������԰����
    /// </summary>
    /// <param name="data">�԰������</param>
    public static void SelectRumble(UIRumbleInfoCtrl ctrl)
    {
        selectRumbleUI = ctrl;
        dropZoneShowUp(true);
    }
    /// <summary>
    /// ��m�԰����(��Ʈ���)
    /// </summary>
    public static void DropRumble()
    {
        selectRumbleUI.UseData();//��Ʈ���
        battlePanelUI.UpdateRumbleList();//��ƸɥR
    }
    /// <summary>
    /// ��������԰����UI
    /// </summary>
    public static void UnSelectRumble()
    {      
        selectRumbleUI = null;
        dropZoneShowUp(false);
    }
    #endregion �԰���T�t��

    #region �C��������T
    /// <summary>
    /// �C��������T���O(�U��)
    /// </summary>
    public static UIGameOverPanelCtrl gameOverPanelCtrl { get; private set; }
    /// <summary>
    /// �]�w�C��������T���O(�U��)
    /// </summary>
    /// <param name="ctrl">���</param>
    public static void SetGameOverPanel(UIGameOverPanelCtrl ctrl)
    {
        gameOverPanelCtrl = ctrl;
    }
    public static void GameOverInfo()
    {
        gameOverPanelCtrl.ShowInfo();
        //�԰�����
        inBattle = false;
        spawnerManager.StopSpawn();
    }

    #endregion �C��������T

}