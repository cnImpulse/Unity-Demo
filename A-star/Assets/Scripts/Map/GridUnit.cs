using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridUnit : MonoBehaviour
{
    public GridUnitData gridUnitData;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void Refresh()
    {
        if (gridUnitData==null)
            Debug.LogError("gridUnitData is null!");

        switch (gridUnitData.gridType)
        {
            case GridType.From:spriteRenderer.color = Color.red;break;
            case GridType.To:spriteRenderer.color = Color.blue;break;
            case GridType.Obstacle: spriteRenderer.color = Color.black;break;

            default: spriteRenderer.color = Color.white;break;
        }

        transform.localPosition = new Vector3((gridUnitData.column + 0.5f) * Metric.gridUnitSide, (gridUnitData.row + 0.5f) * Metric.gridUnitSide, 0);
    }

    //设置颜色
    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }

    public void Reset()
    {
        gridUnitData.gridType = GridType.None;
        Refresh();
    }
}
