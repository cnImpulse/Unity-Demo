using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public GameObject[] stars;
    private bool canSelect = false;

    private void Start()
    {
        if(transform.parent.GetChild(0).name == gameObject.name)
        {
            canSelect = true;
        }
        else
        {
            int beforeNum = int.Parse(gameObject.name) - 1;
            if (PlayerPrefs.GetInt(PlayerPrefs.GetString("nowMap") + "level" + beforeNum.ToString())>0)
                canSelect = true;
        }

        if(canSelect)
        {
            transform.Find("lock").gameObject.SetActive(false);
            transform.Find("bg").gameObject.SetActive(true);

            int count = PlayerPrefs.GetInt(PlayerPrefs.GetString("nowMap") + "level" +gameObject.name);
            if(count >0)
            {
                for(int i=0;i<count;i++)
                {
                    stars[i].SetActive(true);
                }
            }
        }   
    }

    public void Seclected()
    {
        if(canSelect)
        {
            PlayerPrefs.SetString("nowLevel",PlayerPrefs.GetString("nowMap")+"level"+gameObject.name);
            PlayerPrefs.SetInt("nowNum", int.Parse(gameObject.name));
            SceneManager.LoadScene(2);
        }
    }
}
