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
    /// 配合開始UI 執行生怪
    /// </summary>
    public void StartSpawn()
    {
        InvokeRepeating("Spawn", 1f, 5f);
    }
    /// <summary>
    /// 停止產怪
    /// </summary>
    public void StopSpawn()
    {
        CancelInvoke();
    }


    /// <summary>
    /// 產出怪物
    /// </summary>
    void Spawn()
    {
        //參考生怪點位置和方向 : 具現化怪物上場
         CharacterCtrl charCtrl = Instantiate(monster, transform.position, transform.rotation);
        //設定陣營
        charCtrl.SetGroup(PlayerGroup.P2);
        //隨機設定路徑(隨機最小到最大，最大值不會列入)
        charCtrl.SetPath(paths[Random.Range(0, paths.Count)]);
    }
}
