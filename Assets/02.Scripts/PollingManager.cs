using NUnit.Framework;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class PollingManager : Singleton<PollingManager>
{
    public GameObject BombPrefab;
    public int PoolCount;
    public List<GameObject> BombPrefabList;

    private void Start()
    {
        for(int i = 0; i < PoolCount; i++)
        {
            GameObject bombPrefab = Instantiate(BombPrefab,transform);
            bombPrefab.SetActive(false);
            BombPrefabList.Add(bombPrefab);
        }
    }

    public GameObject GetBombPrefab()
    {
        foreach(GameObject obj in BombPrefabList)
        {
            if(obj.activeInHierarchy == false)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        GameObject newObj = Instantiate(BombPrefab, transform); // 또는 다른 기본 프리팹
        BombPrefabList.Add(newObj);
        return newObj;
    }
}
