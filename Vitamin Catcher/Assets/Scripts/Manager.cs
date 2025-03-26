using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public GameObject player;
    public GameObject collectiblesParent;
    public GameObject[] spawnPoints;
    public GameObject[] immunityPrefabs;
    public GameObject[] socialPrefabs;
    public GameObject[] bonePrefabs;
    public GameObject[] virusPrefabs;
    public List<int> immunityList = new List<int>();
    public List<int> socialList = new List<int>();
    public List<int> boneList = new List<int>();
    public int currentLevel = 0;
    public int temp = -1;
    public float delay = 1f;
    public OrbBehaiviour orbMan;
    public TMP_Text notification;
    private bool prevVirus = false;
    void Start()
    {
        DOTween.Init();
        StartCoroutine(SpawnAVitamin());
        Notification("LEVEL IMMUNITY");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }
    public void StartSpawn()
    {
        StartCoroutine(SpawnAVitamin());
    }
    IEnumerator SpawnAVitamin()
    {
        yield return new WaitForSeconds(delay);
        SpawnManager();
    }

    public void SpawnManager()
    {
        int randVirus = Random.Range(0, 10);
        if(randVirus < 7 || prevVirus)
        {
            prevVirus = false;
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
            prevVirus = true;
            Instantiate(virusPrefabs[Random.Range(0,virusPrefabs.Length)], spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity, collectiblesParent.transform);
        }
    }

    public void CheckCollectibleHit()
    {
        if(currentLevel == 0)
        {
            immunityList.Add(temp);
            orbMan.ActivateOrb(currentLevel, (float)immunityList.Count / immunityPrefabs.Length);
            if (immunityList.Count == immunityPrefabs.Length)
            {
                currentLevel = 1;
                Notification("LEVEL UP!\nSOCIAL COGNITION");
            }
            temp = -1;
            StartSpawn();
        }
        else if (currentLevel == 1)
        {
            socialList.Add(temp);
            orbMan.ActivateOrb(currentLevel, (float)socialList.Count / socialPrefabs.Length);
            if (socialList.Count == socialPrefabs.Length)
            {
                currentLevel = 2;
                Notification("LEVEL UP!\nBONES & MUSCLES");
            }
            temp = -1;
            StartSpawn();
        }
        else if (currentLevel == 2)
        {
            boneList.Add(temp);
            orbMan.ActivateOrb(currentLevel, (float)boneList.Count / bonePrefabs.Length);
            if (boneList.Count == bonePrefabs.Length)
            {
                //currentLevel = 2;
                Notification("YOU WIN!");
                Debug.Log("Game Win!!!!!!!!!!!!!");
            }
            else
            {
                temp = -1;
                StartSpawn();
            }
            
        }
    }
    public void VirusHit()
    {
        orbMan.DestroyOrb();
    }

    public void Notification(string noti)
    {
        notification.text = noti;
        notification.color = orbMan.levelColors[currentLevel];
        Sequence mySeq = DOTween.Sequence();
        mySeq.Append(notification.DOFade(1f, 0.5f));
        mySeq.Append(notification.DOFade(0.5f, 0.5f));
        mySeq.Append(notification.DOFade(1f, 0.5f));
        mySeq.Append(notification.DOFade(0.5f, 0.5f));
        mySeq.Append(notification.DOFade(1f, 0.5f));
        mySeq.Append(notification.DOFade(0f, 0.5f));
    }
}
