using UnityEngine;
using UnityEngine.UI;
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
    public GameObject[] cPrefabs;
    public GameObject[] virusPrefabs;
    public List<int> cList = new List<int>();
    public int currentLevel = 0;
    public int temp = -1;
    public float delay = 1f;
    public OrbBehaiviour orbMan;
    public TMP_Text notification;
    private bool prevVirus = false;
    private const int immunityCount = 5;
    private const int socialCount = 7;
    private const int bonesCount = 3;
    private int immunityCatched = 0;
    private int socialCatched = 0;
    private int bonesCatched = 0;
    private int virusHitCount = 0;
    private bool gameOn = true;
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
        if(gameOn)
        {
            StartCoroutine(SpawnAVitamin());
        }
    }
    IEnumerator SpawnAVitamin()
    {
        yield return new WaitForSeconds(delay);
        SpawnManager();
    }

    public void SpawnManager()
    {
        int randVirus = Random.Range(0, 10);
        if(!gameOn)
        {
            return;
        }
        else if ( (randVirus < 7 || prevVirus) && cList.Count < (immunityCount + socialCount + bonesCount))
        {
            prevVirus = false;
            temp = Random.Range(0, cPrefabs.Length);
            while (cList.Contains(temp) )
            {
                temp = Random.Range(0, cPrefabs.Length);
            }
            Instantiate(cPrefabs[temp], spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity, collectiblesParent.transform);
        }
        else if(!prevVirus)
        {
            prevVirus = true;
            Instantiate(virusPrefabs[Random.Range(0, virusPrefabs.Length)], spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity, collectiblesParent.transform);
        }
        StartSpawn();

        /*if(randVirus < 7 || prevVirus)
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
        */

    }

    public void CheckCollectibleHit(int cLevel)
    {
        if(!gameOn)
        {
            return;
        }
        else if(currentLevel == 0 && cLevel == 0 && immunityCatched < immunityCount && !cList.Contains(temp))
        {
            cList.Add(temp);
            immunityCatched++;
            orbMan.ActivateOrb(currentLevel, (float)immunityCatched / immunityCount);
            if (immunityCatched == immunityCount)
            {
                currentLevel = 1;
                Notification("LEVEL UP!\nSOCIAL COGNITION");
            }
            temp = -1;            
        }
        else if (currentLevel == 1 && cLevel == 1 && socialCatched < socialCount && !cList.Contains(temp))
        {
            cList.Add(temp);
            socialCatched++;
            orbMan.ActivateOrb(currentLevel, (float)socialCatched / socialCount);
            if (socialCatched == socialCount)
            {
                currentLevel = 2;
                Notification("LEVEL UP!\nBONES & MUSCLES");
            }
            temp = -1;
        }
        else if (currentLevel == 2 && cLevel == 2 && bonesCatched < bonesCount && !cList.Contains(temp))
        {
            cList.Add(temp);
            bonesCatched++;
            orbMan.ActivateOrb(currentLevel, (float)bonesCatched / bonesCount);
            if (bonesCatched == bonesCount)
            {
                //currentLevel = 2;
                Notification("YOU WIN!");
                Debug.Log("Game Win!!!!!!!!!!!!!");
            }
            else
            {
                temp = -1;
            }
            
        }
        //StartSpawn();
    }
    public void VirusHit()
    {
        orbMan.DestroyOrb();
        virusHitCount++;
        if(virusHitCount == 3)
        {
            gameOn = false;
            Notification("Game Over!");
        }
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
