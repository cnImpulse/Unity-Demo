using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Navigator
{
    private class NavigationData
    {
        //该节点是否可通行
        public bool open;
        //起点到该节点的消耗
        public int G;
        //该节点到终点的估计消耗
        public int H;
        //启发值F=G+H
        public int F { get { return G + H; } }

        //当前节点信息
        public GridUnitData thisGrid;
        //上一个导航节点
        public NavigationData preGrid;

        public NavigationData()
        {
            Reset();
        }

        //重置导航数据信息
        public void Reset()
        {
            open = true;
            G = 0;
            H = 0;

            thisGrid = null;
            preGrid = null;
        }
    }

    //单例
    private static Navigator instance;
    public static Navigator Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Navigator();
                instance.Init();
            }
            return instance;
        }
    }

    //对象池
    private int curUsedIdx = 0;
    private List<NavigationData> navigationDataPool = null;

    //重置对象池
    private void ResetPool()
    {
        for (int i = 0; i < curUsedIdx; ++i)
        {
            navigationDataPool[i].Reset();
        }
        curUsedIdx = 0;
    }

    //建立对象池
    private void Init()
    {
        //初始化一定数量的导航数据
        navigationDataPool = new List<NavigationData>(999);
        for (int i = 0; i < 999; ++i)
        {
            navigationDataPool.Add(new NavigationData());
        }
    }

    //取空的导航数据
    private NavigationData GetEmptyNavigationData(GridUnitData thisGrid,NavigationData preGrid,int G,int H)
    {
        //优先从池中取
        NavigationData nd = null;
        if (curUsedIdx < navigationDataPool.Count)
        {
            nd = navigationDataPool[curUsedIdx];
        }
        else
        {
            nd = new NavigationData();
            navigationDataPool.Add(nd);
        }
        ++curUsedIdx;

        nd.open = true;
        nd.G = G;
        nd.H = H;
        nd.thisGrid = thisGrid;
        nd.preGrid = preGrid;

        return nd;
    }

    ///<summary>
    ///导航
    ///</summary>
    ///<param name="map">地图数据</param>
    ///<param name="from">起点</param>
    ///<param name="to">终点</param>
    ///<param name="path">路径</param>
    ///<param name="searched">搜索过但为采用的节点</param>
    public bool Navigate(Map map, GridUnitData from, GridUnitData to, out List<GridUnitData> path, out List<GridUnitData> searched)
    {
        //输出参数赋初值
        path = new List<GridUnitData>();
        searched = new List<GridUnitData>();

        //没有地图则结束导航
        if (map == null)
        {
            Debug.Log("mapData is null!");
            return false;
        }
        if (from == null || to == null)
        {
            Debug.Log("from or to is null!");
            return false;
        }


        //open储存将要探索的节点
        List<NavigationData> open = new List<NavigationData>();
        //close储存已经探索的节点
        List<NavigationData> close = new List<NavigationData>();

        //1.把起点加入open
        open.Add(GetEmptyNavigationData(from, null, 0, from.Distance(to)));

        int trytimes = 999;
        while (trytimes--!=0)
        {
            //2.判断open列表,如果为空则搜索失败,如果终点在里面则搜索成功
            if (open.Count == 0)
                return false;
            NavigationData temp = Exits(open, to);
            if (temp != null)
            {
                //从终点回溯至起点
                while (temp != null)
                {
                    path.Add(temp.thisGrid);
                    temp = temp.preGrid;
                }
                return true;
            }

            //3.从open列表取出F值最小的节点,将其设为当前节点,并加入close列表中
            NavigationData nowGrid = open[0];
            int minF = open[0].F, minH = open[0].H;
            for(int i = 1; i < open.Count; ++i)
            {
                if (open[i].F < minF || (open[i].F == minF && open[i].H < minH)) 
                {
                    nowGrid = open[i];
                    minF = open[i].F;
                    minH = open[i].H;
                }
            }
            close.Add(nowGrid);
            open.Remove(nowGrid);

            map.gridUnits[nowGrid.thisGrid.row, nowGrid.thisGrid.column].SetColor(Color.green);
            Debug.Log(nowGrid.thisGrid.row + "|" + nowGrid.thisGrid.column + "      " + nowGrid.G + ',' + nowGrid.H + ',' + nowGrid.F);

            //4.获取当前节点的所有可到达节点
            List<GridUnitData> neighbor = map.currentMapData.GetNeighbor(nowGrid.thisGrid);
            //对于每个节点
            for(int i = 0; i < neighbor.Count; ++i)
            {
                //4.1 如果该节点在close列表中,删除open中的该节点
                NavigationData t = Exits(close, neighbor[i]);
                if (t != null)
                {
                    open.Remove(t);
                    continue;
                }

                t = Exits(open, neighbor[i]);
                int G = nowGrid.G + 1;
                int H = to.Distance(neighbor[i]);
                //4.2 如果该节点不在open中,计算G,H值,设置父节点为当前节点,加入open列表
                if (t == null)
                {
                    NavigationData target = GetEmptyNavigationData(neighbor[i], nowGrid, G, H);
                    open.Add(target);
                }
                //4.3 如果该节点在open中且以当前节点求F值更小,则更新G,H,设父节点为当前节点
                else if (t.F > G + H)
                {
                    t.G = G;
                    t.H = H;
                    t.preGrid = nowGrid;
                }

                map.gridUnits[neighbor[i].row, neighbor[i].column].SetColor(Color.yellow);
            }
        }
        return false;
    }

    //查看是否节点在列表中
    private NavigationData Exits(List<NavigationData> list, GridUnitData target)
    {
        for (int i = 0; i < list.Count; ++i)
        {
            if (list[i].thisGrid == target)
                return list[i];
        }
        return null;
    }
}
