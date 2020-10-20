using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    public void MapReset()
    {
        Map.Instance.Reset();
    }

    //设置单元格类型
    public void SetGridType(int type)
    {
        switch (type)
        {
            case 0:Map.Instance.selectType = GridType.From;break;
            case 1: Map.Instance.selectType = GridType.To; break;
            case 2: Map.Instance.selectType = GridType.Obstacle; break;

            default: Map.Instance.selectType = GridType.None;break;
        }
        Map.Instance.last = null;
    }

    //开始导航
    public void Navigate()
    {
        Map.Instance.NavigateTest();
    }
}
