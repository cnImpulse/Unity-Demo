using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSelect : MonoBehaviour
{
    public int starsNum = 0;
    private bool canSelect = false;
    public int levelnum = 8;

    public GameObject locks;
    public GameObject stars;
    public GameObject panel;
    public GameObject map;
    public Text starstxt;

    private void Start()
    {
        //PlayerPrefs.DeleteAll();

        if (PlayerPrefs.GetInt("totalStars",0) >= starsNum)
        {
            canSelect = true;
        };

        if(canSelect)
        {
            locks.SetActive(false);
            stars.SetActive(true);

            int s = 0;
            for (int i = 1; i <= levelnum; i++)
                s += PlayerPrefs.GetInt(gameObject.name+"level"+i.ToString(),0);
            int all = levelnum * 3;
            starstxt.text = s.ToString() + "/" + all.ToString();
        }
    }

    public void Selected()
    {
        if(canSelect)
        {
            PlayerPrefs.SetString("nowMap",gameObject.name);
            panel.SetActive(true);
            map.SetActive(false);
        }
    }
}
