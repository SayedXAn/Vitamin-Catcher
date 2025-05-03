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
    public GameObject confetti;
    public GameObject[] spawnPoints;
    public GameObject[] cPrefabs;
    public GameObject[] virusPrefabs;
    public Image[] hearts;    
    public int currentLevel = 0;
    public int temp = -1;
    public float delay = 1f;
    public OrbBehaiviour orbMan;
    public TMP_Text notification;
    public Image notificationBG;
    private bool prevVirus = false;
    private int virusHitCount = 0;
    private int prevC = -1;
    private int[] levelIndex = {0, 5, 8, 15};
    public int[] vitCatched = {0, 0, 0};
    private int[] vitCount = {5, 3, 7};
    private List<int> cList = new List<int>();
    private List<int> tempC = new List<int>();
    private bool gameOn = false;
    public AudioSource bgm;
    public AudioSource sfx;
    public AudioClip[] clips;
    public int timer = 60;
    public GameObject timerParent;
    public Image timerFill;
    public TMP_Text timerText;
    public int score = 0;
    public int hiScore = 0;
    public TMP_Text hiScoreText;
    public TMP_Text playerScoreText;

    void Start()
    {
        DOTween.Init();
        hiScore = 0;

        if(PlayerPrefs.GetInt("highScore") > 0)
        {
            hiScore = PlayerPrefs.GetInt("highScore");
        }
        UpdateScore();
        confetti.SetActive(false);
        if(!bgm.isPlaying)
        {
            bgm.Play();
        }
        FillList(levelIndex[currentLevel], levelIndex[currentLevel + 1]);
    }

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
        StartTimer();
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
    public void FillList(int x, int y)
    {
        for(int i = x;  i < y; i++)
        {
            tempC.Add(i);
        }
    }
    public void SpawnManager()
    {
        int randVirus = Random.Range(0, 10);
        if(!gameOn)
        {
            return;
        }
        else if ( (randVirus < 7 || prevVirus) && cList.Count < (vitCount[0] + vitCount[1] + vitCount[2]))
        {
            prevVirus = false;
            temp = Random.Range(0, tempC.Count);
            while(tempC.Count > 1 && temp == prevC)
            {
                temp = Random.Range(0, tempC.Count);
            }
            prevC = temp;
            Instantiate(cPrefabs[tempC[temp]], spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity, collectiblesParent.transform);
        }
        else if(!prevVirus)
        {
            prevVirus = true;
            Instantiate(virusPrefabs[Random.Range(0, virusPrefabs.Length)], spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity, collectiblesParent.transform);
        }
        StartSpawn();
    }

    public void CheckCollectibleHit(int cLevel, int cID)
    {
        
        if(!gameOn)
        {
            return;
        }
        else if(currentLevel == cLevel  && vitCatched[currentLevel] < vitCount[currentLevel] && !cList.Contains(cID))
        {
            sfx.clip = clips[0];
            sfx.Play();
            cList.Add(cID);
            tempC.Remove(cID);
            vitCatched[currentLevel]++;
            orbMan.ActivateOrb(currentLevel, (float)vitCatched[currentLevel] / vitCount[currentLevel]);
            score += 5;            
            if (vitCatched[currentLevel] == vitCount[currentLevel])
            {
                currentLevel++;
                
                StartCoroutine(LevelUpConfetti());
                if(currentLevel == 1)
                {
                    score += 50;
                    DestroyOldVitamins("immunity");
                    Notification("LEVEL UP!\nBONES & MUSCLES\nLevel Up Bonus 50");
                    tempC = new List<int>();
                    FillList(levelIndex[currentLevel], levelIndex[currentLevel + 1]);
                }
                else if(currentLevel == 2)
                {
                    score += 100;
                    DestroyOldVitamins("bones");
                    Notification("LEVEL UP!\nSOCIAL COGNITION\nLevel Up Bonus 100");
                    tempC = new List<int>();
                    FillList(levelIndex[currentLevel], levelIndex[currentLevel + 1]);
                }
                else if(currentLevel == 3)
                {
                    score += 200 + timer;
                    DestroyOldVitamins("social");
                    gameOn = false;
                    confetti.SetActive(true);
                    currentLevel = 2;
                    string winTextTemp = "YOU WIN!\nWin Bonus " + (200 + timer) + "Your Score: " + score;
                    if(score > hiScore)
                    {
                        hiScore  = score;
                        hiScoreText.text = score.ToString();
                        PlayerPrefs.SetInt("highScore", hiScore);
                        winTextTemp += "\n(High Score)";
                    }
                    Notification(winTextTemp);
                    Debug.Log("Game Win!!!!!!!!!!!!!");
                    StartCoroutine(ResetGame());
                }
                
            }
            UpdateScore();
            temp = -1;            
        }
        
        else
        {
            Debug.Log("tui ke? " + cID);
            sfx.clip = clips[1];
            sfx.Play();
        }
    }
    public void VirusHit()
    {
        if (!gameOn)
        {
            return;
        }
        sfx.clip = clips[2];
        sfx.Play();
        orbMan.DestroyOrb();
        virusHitCount++;
        hearts[virusHitCount-1].gameObject.SetActive(false);
        if(virusHitCount == 3)
        {
            GameOver();
            UpdateScore();
        }
    }

    public void DestroyOldVitamins(string levelTag)
    {
        foreach(GameObject oldVit in GameObject.FindGameObjectsWithTag(levelTag))
        {
            Destroy(oldVit);
        }
    }

    public void Notification(string noti)
    {
        UpdateScore();
        notification.text = noti;
        notification.color = orbMan.levelColors[currentLevel];
        Sequence mySeq = DOTween.Sequence();
        mySeq.Append(notificationBG.DOFade(0.4f, 0.2f));
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

    IEnumerator LevelUpConfetti()
    {
        confetti.SetActive(true);
        yield return new WaitForSeconds(2);
        confetti.SetActive(false);
    }

    public void StartTimer()
    {
        StartCoroutine(TimerCount());
    }

    IEnumerator TimerCount()
    {
        if(gameOn && timer > 0)
        {
            yield return new WaitForSeconds(1);
            timer--;
            timerText.text = timer.ToString();
            timerFill.DOFillAmount(timer/60f, 1f);
            if(timer == 0 && gameOn)
            {
                GameOver();
            }
            else
            {
                StartCoroutine(TimerCount());
            }
        }
    }

    public void GameOver()
    {
        gameOn = false;
        if (score > hiScore)
        {
            hiScore = score;            
            PlayerPrefs.SetInt("highScore", hiScore);
        }
        UpdateScore();
        Notification("Game Over!");
        DestroyOldVitamins("immunity");
        DestroyOldVitamins("bones");
        DestroyOldVitamins("social");
        DestroyOldVitamins("virus");
        StartCoroutine(ResetGame());
    }
    public void UpdateScore()
    {
        playerScoreText.text = "Your Score: " + score.ToString();
        hiScoreText.text = "High Score: " + hiScore.ToString();
    }
}
