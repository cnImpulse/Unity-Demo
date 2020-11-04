using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelAsync : MonoBehaviour
{

	void Start ()
    {
        Screen.SetResolution(1200,675, false);
        Invoke("Load", 2);
	}

    void Load()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
