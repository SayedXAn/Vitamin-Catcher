
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Manager : MonoBehaviour
{
    public GameObject player;
    public GameObject collectiblesParent;
    public GameObject[] spawnPoints;
    public GameObject[] immunityPrefabs;
    public GameObject[] socialPrefabs;
    public GameObject[] bonePrefabs;
    public GameObject[] virusPrefabs;
    public int immunityCount = 0;
    public int socialCount = 0;
    public int boneCount = 0;
    public List<int> immunityList = new List<int>();
    public List<int> socialList = new List<int>();
    public List<int> boneList = new List<int>();
    public int currentLevel = 0;
    public int temp = -1;
    void Start()
    {
        StartCoroutine(SpawnAVitamin());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartSpawn()
    {
        StartCoroutine(SpawnAVitamin());
    }
    IEnumerator SpawnAVitamin()
    {
        yield return new WaitForSeconds(1.5f);
        SpawnManager();
    }

    public void SpawnManager()
    {
        int randVirus = Random.Range(0, 10);
        if(randVirus < 8)
        {
            if (currentLevel == 0)
            {
                temp = Random.Range(0, immunityPrefabs.Length);
                while (immunityList.Contains(temp))
                {
                    temp = Random.Range(0, immunityPrefabs.Length);
                }
                Instantiate(immunityPrefabs[temp], spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity, collectiblesParent.transform);
            }
            else if (currentLevel == 1)
            {
                temp = Random.Range(0, socialPrefabs.Length);
                while (socialList.Contains(temp))
                {
                    temp = Random.Range(0, socialPrefabs.Length);
                }
                Instantiate(socialPrefabs[temp], spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity, collectiblesParent.transform);
            }
            else if (currentLevel == 2)
            {
                temp = Random.Range(0, bonePrefabs.Length);
                while (boneList.Contains(temp))
                {
                    temp = Random.Range(0, bonePrefabs.Length);
                }
                Instantiate(bonePrefabs[temp], spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity, collectiblesParent.transform);
            }
        }
        else
        {
            Instantiate(virusPrefabs[Random.Range(0,virusPrefabs.Length)], spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity, collectiblesParent.transform);
        }
    }

    public void CheckCollectibleHit()
    {
        if(currentLevel == 0)
        {
            immunityList.Add(temp);
            if(immunityList.Count == immunityPrefabs.Length)
            {
                currentLevel = 1;
            }
            temp = -1;
            StartSpawn();
        }
        else if (currentLevel == 1)
        {
            socialList.Add(temp);
            if (socialList.Count == socialPrefabs.Length)
            {
                currentLevel = 2;
            }
            temp = -1;
            StartSpawn();
        }
        else if (currentLevel == 2)
        {
            boneList.Add(temp);
            if (boneList.Count == bonePrefabs.Length)
            {
                //currentLevel = 2;
                Debug.Log("Game Win!!!!!!!!!!!!!");
            }
            else
            {
                temp = -1;
                StartSpawn();
            }
            
        }
    }
}
