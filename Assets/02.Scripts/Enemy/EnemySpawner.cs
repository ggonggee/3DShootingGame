using System.Collections;

using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public float SpawnMinTime = 3f;
    public float SpawnMaxTime = 7f;
    public float Radius = 5f;

    private void Start()
    {
        StartCoroutine(EnemySpawn());
    }
    IEnumerator EnemySpawn()
    {
        Vector3 ranPos =Random.insideUnitSphere* Radius + transform.position;
        float ran = Random.Range(SpawnMinTime, SpawnMaxTime);
        yield return new WaitForSeconds(ran);
        Instantiate(EnemyPrefab, ranPos, Quaternion.identity);
        StartCoroutine(EnemySpawn());
    }



}
