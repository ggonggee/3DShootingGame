using System.Collections;

using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;

    private void Start()
    {
        StartCoroutine(EnemySpawn());
    }
    IEnumerator EnemySpawn()
    {
        float ran = Random.Range(1f, 3f);
        yield return new WaitForSeconds(ran);
        Instantiate(EnemyPrefab,transform.position,Quaternion.identity);
        StartCoroutine(EnemySpawn());
    }



}
