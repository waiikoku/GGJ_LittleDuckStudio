using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    public GameObject[] prefabs;
    public Transform[] spawnPoints;
    public Transform[] focusPoints;
    public Transform[] lockNWave;
    public Transform confine;
    public int[] zoneAmount;
    public int level = 0;
    public CinemachineVirtualCamera playerVC;
    public CinemachineVirtualCamera focusVC;
    public Camera cam;
    public Transform player;
    private int spawnedCount;
    public Vector2 viewpointLeft;
    public Vector2 viewpointRight;
    public float minDelay;
    public float maxDelay;

    public GameObject iviWall;

    [SerializeField] private GameObject boss;
    public void TriggerZone()
    {
        Spawn();
    }

    private void Spawn()
    {
        if (level == lockNWave.Length - 1)
        {
            boss.SetActive(true);
            iviWall.SetActive(true);
            SoundManager.Instance.PlayBGM("Burning Soul");
            return;
        }
        if (spawnPoints.Length < level) return;
        /*
        focusVC.Follow = focusPoints[level];
        playerVC.gameObject.SetActive(false);
        focusVC.gameObject.SetActive(true);
        */
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
        //lockNWave[level].GetComponent<BoxCollider2D>().isTrigger = false;
    }

    private GameObject goSpawn(Vector2 position)
    {
        GameObject go = Instantiate(prefabs[Random.Range(0, prefabs.Length)], position, Quaternion.identity);
        return go;
    }

    public void ConfirmKill()
    {
        spawnedCount--;
        if(spawnedCount == 0)
        {
            focusVC.gameObject.SetActive(false);
            playerVC.gameObject.SetActive(true);
            if (level > lockNWave.Length - 1) return;
            //lockNWave[level].GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }
}
