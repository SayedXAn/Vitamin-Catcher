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
    public Image[] hearts;
    public List<int> cList = new List<int>();
    public int currentLevel = 0;
    public int temp = -1;
    public float delay = 1f;
    public OrbBehaiviour orbMan;
    public TMP_Text notification;
    public Image notificationBG;
    private bool prevVirus = false;
    private const int immunityCount = 5;
    private const int socialCount = 7;
    private const int bonesCount = 3;
    private int immunityCatched = 0;
    private int socialCatched = 0;
    private int bonesCatched = 0;
    private int virusHitCount = 0;
    private int prevC = -1;
    private bool gameOn = false;
    void Start()
    {
        DOTween.Init();        
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
            RestartGame();
        }
    }
    public void StartButton()
    {
        gameOn = true;
        StartCoroutine(SpawnAVitamin());
        Notification("LEVEL IMMUNITY");
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
            while (cList.Contains(temp) || (cList.Count < cPrefabs.Length-1 && temp == prevC))
            {
                temp = Random.Range(0, cPrefabs.Length);
            }
            prevC = temp;
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

    public void CheckCollectibleHit(int cLevel, int cID)
    {
        if(!gameOn)
        {
            return;
        }
        else if(currentLevel == 0 && cLevel == 0 && immunityCatched < immunityCount && !cList.Contains(cID))
        {
            cList.Add(cID);
            immunityCatched++;
            orbMan.ActivateOrb(currentLevel, (float)immunityCatched / immunityCount);
            if (immunityCatched == immunityCount)
            {
                currentLevel = 1;
                Notification("LEVEL UP!\nSOCIAL COGNITION");
            }
            temp = -1;            
        }
        else if (currentLevel == 1 && cLevel == 1 && socialCatched < socialCount && !cList.Contains(cID))
        {
            cList.Add(cID);
            socialCatched++;
            orbMan.ActivateOrb(currentLevel, (float)socialCatched / socialCount);
            if (socialCatched == socialCount)
            {
                currentLevel = 2;
                Notification("LEVEL UP!\nBONES & MUSCLES");
            }
            temp = -1;
        }
        else if (currentLevel == 2 && cLevel == 2 && bonesCatched < bonesCount && !cList.Contains(cID))
        {
            cList.Add(cID);
            bonesCatched++;
            orbMan.ActivateOrb(currentLevel, (float)bonesCatched / bonesCount);
            if (bonesCatched == bonesCount)
            {
                gameOn = false;
                //currentLevel = 2;
                Notification("YOU WIN!");
                Debug.Log("Game Win!!!!!!!!!!!!!");
                StartCoroutine(ResetGame());
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
        if (!gameOn)
        {
            return;
        }
        orbMan.DestroyOrb();
        virusHitCount++;
        hearts[virusHitCount-1].gameObject.SetActive(false);
        if(virusHitCount == 3)
        {
            gameOn = false;
            Notification("Game Over!");
            StartCoroutine(ResetGame());
        }
    }

    public void Notification(string noti)
    {
        
        notification.text = noti;
        notification.color = orbMan.levelColors[currentLevel];
        Sequence mySeq = DOTween.Sequence();
        mySeq.Append(notificationBG.DOFade(1, 0.2f));
        mySeq.Append(notification.DOFade(1f, 0.5f));
        mySeq.Append(notification.DOFade(0.5f, 0.5f));
        mySeq.Append(notification.DOFade(1f, 0.5f));
        mySeq.Append(notification.DOFade(0.5f, 0.5f));
        mySeq.Append(notification.DOFade(1f, 0.5f));
        mySeq.Append(notification.DOFade(0f, 0.5f));
        mySeq.Append(notificationBG.DOFade(0, 0.2f));

    }

    IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(3);
        RestartGame();
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
