using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicked : MonoBehaviour
{
    public GameObject hide;
    public GameObject show;

    public void Click()
    {
        show.SetActive(true);
        hide.SetActive(false);
    }
}
