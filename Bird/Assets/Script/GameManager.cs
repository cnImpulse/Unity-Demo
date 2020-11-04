using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<Bird> birds;
    public List<Pig> pigs;
    public static GameManager _instance;
    public GameObject lose;
    public GameObject win;
    public GameObject level;
    public GameObject[] stars;
    
    private Vector3 originPos;
    private int starsNum = 0;

    private void Awake()
    {
        _instance = this;
        if(birds.Count > 0)
            originPos = birds[0].transform.position;
    }

    private void Start()
    {
        Initialized();
    }

    private void Update()
    {
        if (pigs.Count == 0)
        {
            PausePanel.canPause = false;
            Invoke("Win", 1.5f);
            starsNum = birds.Count + 1;
        }
    }

    private void Initialized()
    {
        for(int i = 0;i<birds.Count;i++)
        {
            if(i ==0 )
            {
                birds[i].transform.position = originPos;
                birds[i].enabled = true;
                birds[i].sp.enabled = true;
                birds[i].canMove = true;
            }
            else
            {
                birds[i].enabled = false;
                birds[i].sp.enabled = false;
                birds[i].canMove = false;
            }
        }
    }

    public void NextBird()
    {
        if(pigs.Count > 0)
        {
            if(birds.Count > 0)
            {
                Initialized();
            }
            else
            {
                PausePanel.canPause = false;
                Invoke("Lose", 1.5f);
            }
        }
    }

    public void ShowStars()
    {
        StartCoroutine("show");
    }

    IEnumerator show()
    {
        for (int i=0; i < birds.Count + 1; i++)
        {
            if (i >= stars.Length)
                break;
            yield return new WaitForSeconds(0.2f);
            stars[i].SetActive(true);
        }
    }

    public void Replay()
    {
        SaveData();
        SceneManager.LoadScene(2);
    }

    public void Home()
    {
        SaveData();
        SceneManager.LoadScene(1);
    }

    public void SaveData()
    {
        if (starsNum > PlayerPrefs.GetInt(PlayerPrefs.GetString("nowLevel"),0))
        {
            int sum = PlayerPrefs.GetInt("totalStars");
            sum += starsNum-PlayerPrefs.GetInt(PlayerPrefs.GetString("nowLevel"));
            print(sum);
            PlayerPrefs.SetInt("totalStars", sum);
            PlayerPrefs.SetInt(PlayerPrefs.GetString("nowLevel"), starsNum);
        } 
    }

    public void Next()
    {
        SaveData();
        int next = PlayerPrefs.GetInt("nowNum")+1;

        if (next <= 8)
        {
            PlayerPrefs.SetInt("nowNum", next);
            PlayerPrefs.SetString("nowLevel", PlayerPrefs.GetString("nowMap") + "level" + next.ToString());
            Instantiate(Resources.Load(PlayerPrefs.GetString("nowLevel")));
            Destroy(level);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    public void Win()
    {
        win.SetActive(true);
    }
    public void Lose()
    {
        lose.SetActive(true);
    }
}

