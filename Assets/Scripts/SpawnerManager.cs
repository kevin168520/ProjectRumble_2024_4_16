using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public CharacterCtrl monster;

    public List<PathCtrl> paths;

    private void OnEnable()
    {
        GameSystem.SetSpawnerManager(this);
    }

    private void OnDisable()
    {
        GameSystem.SetSpawnerManager(null);
    }

    /// <summary>
    /// �t�X�}�lUI ����ͩ�
    /// </summary>
    public void StartSpawn()
    {
        InvokeRepeating("Spawn", 1f, 5f);
    }
    /// <summary>
    /// �����
    /// </summary>
    public void StopSpawn()
    {
        CancelInvoke();
    }


    /// <summary>
    /// ���X�Ǫ�
    /// </summary>
    void Spawn()
    {
        //�Ѧҥͩ��I��m�M��V : ��{�ƩǪ��W��
         CharacterCtrl charCtrl = Instantiate(monster, transform.position, transform.rotation);
        //�]�w�}��
        charCtrl.SetGroup(PlayerGroup.P2);
        //�H���]�w���|(�H���̤p��̤j�A�̤j�Ȥ��|�C�J)
        charCtrl.SetPath(paths[Random.Range(0, paths.Count)]);
    }
}
