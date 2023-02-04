using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    public GameObject[] prefabs;
    public Transform[] spawnPoints;
    public Transform[] focusPoints;
    public int[] zoneAmount;
    public int level = 0;
<<<<<<< HEAD
    public CinemachineVirtualCamera playerVC;
    public CinemachineVirtualCamera focusVC;
=======
    public CinemachineVirtualCamera cmVC;
>>>>>>> main
    public Camera cam;
    public Transform player;
    private int spawnedCount;
    public Vector2 viewpointLeft;
    public Vector2 viewpointRight;
    public float minDelay;
    public float maxDelay;
    public void TriggerZone()
    {
        Spawn();
    }

    private void Spawn()
    {
        if (spawnPoints.Length < level) return;
<<<<<<< HEAD
        focusVC.Follow = focusPoints[level];
        playerVC.gameObject.SetActive(false);
        focusVC.gameObject.SetActive(true);
=======
        cmVC.Follow = focusPoints[level];
>>>>>>> main
        StartCoroutine(SpawnThread());
    }

    private IEnumerator SpawnThread()
    {
        int count = 0;
        while (count < (float)zoneAmount[level] / 2f)
        {
            Vector2 randomLeft = cam.ViewportToWorldPoint(viewpointLeft);
            Vector2 randomRight = cam.ViewportToWorldPoint(viewpointRight);
            GameObject go = goSpawn(randomLeft);
            //GameObject go = Instantiate(prefabs[Random.Range(0, prefabs.Length)], (Vector2)spawnPoints[level].position + Random.insideUnitCircle * Random.Range(0f,3f), Quaternion.identity);
            CharacterLayerManager.Instance.Add(go.GetComponentInChildren<SpriteRenderer>());
            GameObject go1 = goSpawn(randomRight);
            CharacterLayerManager.Instance.Add(go1.GetComponentInChildren<SpriteRenderer>());
            count += 2;
            spawnedCount += 2;
            yield return new WaitForSeconds(Random.Range(minDelay,maxDelay));
        }
        level++;
    }

    private GameObject goSpawn(Vector2 position)
    {
        GameObject go = Instantiate(prefabs[Random.Range(0, prefabs.Length)], position + Random.insideUnitCircle * Random.Range(0f, 3f), Quaternion.identity);
        return go;
    }

    public void ConfirmKill()
    {
        spawnedCount--;
        if(spawnedCount == 0)
        {
<<<<<<< HEAD
            focusVC.gameObject.SetActive(false);
            playerVC.gameObject.SetActive(true);
=======
            cmVC.Follow = player;
>>>>>>> main
        }
    }
}
