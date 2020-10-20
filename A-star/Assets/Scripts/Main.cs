using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] private int width = 20, height = 36;

    private void Start()
    {
        Map.Instance.LoadMapData(new MapData(width, height));
    }
}
