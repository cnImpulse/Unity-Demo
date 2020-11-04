using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    public GameObject button;
    [HideInInspector] public static bool canPause;

    private Animator anim;

    private void Awake()
    {
        canPause = true;
        anim = GetComponent<Animator>();
    }
    
    public void Retry()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    public void Pause()
    {
        if (!canPause)
            return;
        anim.SetBool("isPause", true);
        button.SetActive(false);

        if(GameManager._instance.birds.Count >0)
        {
            if (GameManager._instance.birds[0].isFly ==false)
                GameManager._instance.birds[0].canMove = false;
        }
    }

    public void Home()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        anim.SetBool("isPause", false);

        if (GameManager._instance.birds.Count > 0)
        {
            if (GameManager._instance.birds[0].isFly == false)
                GameManager._instance.birds[0].canMove = true;
        }
    }

    public void PauseAnimEnd()
    {
        Time.timeScale = 0;
    }

    public void ResumeAnimEnd()
    {
        button.SetActive(true);
    }
}
