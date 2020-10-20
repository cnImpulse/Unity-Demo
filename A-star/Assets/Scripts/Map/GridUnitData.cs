using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GridType{
    From,
    To,
    Obstacle,
    None, //没有类型
}

public class GridUnitData
{
    public GridType gridType;
    public int row;
    public int column;

    GridUnitData()
    {
        gridType = GridType.None;
    }

    public GridUnitData(int row,int column):this()
    {
        this.row = row;
        this.column = column;
    }

    //该单元格到目标单元格的曼哈顿距离
    public int Distance(GridUnitData target)
    {
        return Mathf.Abs(row - target.row) + Mathf.Abs(column - target.column);
    }
}
