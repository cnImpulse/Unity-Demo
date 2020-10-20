using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private void Start()
    {
        Map.Instance.LoadMapData(new MapData(20, 36));
    }
}
