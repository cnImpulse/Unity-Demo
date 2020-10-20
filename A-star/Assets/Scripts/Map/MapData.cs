using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData
{
    //地图宽高
    public int mapWidth;
    public int mapHeight;
    //所有单元格数据
    public GridUnitData[,] gridUnitDatas;

    MapData() { }

    public MapData(int height,int width)
    {
        mapWidth = width;
        mapHeight = height;
        gridUnitDatas = new GridUnitData[mapHeight, mapWidth];
        for(int i = 0; i < mapHeight; ++i)
        {
            for(int j = 0; j < mapWidth; ++j)
            {
                gridUnitDatas[i, j] = new GridUnitData(i,j);
            }
        }
    }

    //获取目标的所有可到达的相邻节点
    public List<GridUnitData> GetNeighbor(GridUnitData target)
    {
        //定义方向数组
        int[] dx = { 0, 1, 0, -1 };
        int[] dy = { 1, 0, -1, 0 };
        
        List<GridUnitData> neighbor = new List<GridUnitData>();

        for(int i = 0; i < 4; ++i)
        {
            int x = target.row + dx[i], y = target.column + dy[i];
            //越界或者是障碍则跳过
            if (x < 0 || x >= mapHeight || y < 0 || y >= mapWidth || gridUnitDatas[x, y].gridType == GridType.Obstacle)
                continue;
            neighbor.Add(gridUnitDatas[x, y]);
        }
        return neighbor;
    }
}
