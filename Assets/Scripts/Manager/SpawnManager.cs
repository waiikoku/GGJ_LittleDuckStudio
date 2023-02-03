using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    public GameObject[] prefabs;
    public Transform[] zonePoints;
    public Transform[] spawnPoints;
    public int[] zoneAmount;
    public int level = 0;

    public void TriggerZone()
    {
        Spawn();
    }

    private void Spawn()
    {
        StartCoroutine(SpawnThread());
    }

    private IEnumerator SpawnThread()
    {
        int count = 0;
        while (count < zoneAmount[level])
        {
            GameObject go = Instantiate(prefabs[Random.Range(0, prefabs.Length)], (Vector2)spawnPoints[level].position + Random.insideUnitCircle * 3, Quaternion.identity);
            CharacterLayerManager.Instance.Add(go.GetComponentInChildren<SpriteRenderer>());
            count++;
            yield return new WaitForSeconds(Random.Range(0.1f,0.75f));
        }
        level++;
    }
}
