using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Map : MonoBehaviour
{
    private static Map instance;
    public static Map Instance
    {
        get
        {
            if (!instance)
            {
                instance = new Map();
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public MapData currentMapData;
    public GridUnit[,] gridUnits;

    [SerializeField] private CameraControl mapCamera;
    [SerializeField] private Transform gridUnitRoot;
    [SerializeField] private GridUnit gridUnitPrefab;

    //加载地图数据
    public void LoadMapData(MapData mapData)
    {
        if (mapData == null)
        {
            Debug.Log("mapData is null!");
        }
        currentMapData = mapData;
        Genrate();
    }

    //根据地图数据生成地图
    private void Genrate()
    {
        gridUnits = new GridUnit[currentMapData.mapHeight, currentMapData.mapWidth];

        for(int i = 0; i < currentMapData.mapHeight; ++i)
        {
            for(int j = 0; j < currentMapData.mapWidth; ++j)
            {
                gridUnits[i, j] = InstantiateGridUnit(i, j);
            }
        }
    }

    //实例化单元格
    private GridUnit InstantiateGridUnit(int row,int column)
    {
        GridUnit gu = Instantiate(gridUnitPrefab,gridUnitRoot);
        gu.gridUnitData = currentMapData.gridUnitDatas[row, column];
        gu.name = string.Format("GridUnit_{0}_{1}",row,column);
        gu.Refresh();
        return gu;
    }

    //重置地图
    public void Reset()
    {
        for(int i = 0; i < currentMapData.mapHeight; ++i)
        {
            for(int j = 0; j < currentMapData.mapWidth; ++j)
            {
                gridUnits[i, j].Reset();
            }
        }
        mapCamera.ResetCamera();
    }

    //点击单元格
    private GridUnit SelectGrid()
    {
        if (!Input.GetMouseButton(0)|| EventSystem.current.IsPointerOverGameObject())
            return null;
        Vector3 clickPos = mapCamera.camera.ScreenToWorldPoint(Input.mousePosition);
        clickPos.z = 0;
        GridUnit clicked = GetClickedGrid(clickPos);
        return clicked;
    }

    //通过世界坐标获得对应对应单元格,没有单元格返回null.
    private GridUnit GetClickedGrid(Vector3 pos)
    {
        //世界坐标转相对根节点的局部坐标
        pos = gridUnitRoot.transform.InverseTransformPoint(pos);
        //局部坐标转网格坐标
        int row = Mathf.FloorToInt(pos.y / Metric.gridUnitSide);
        int column = Mathf.FloorToInt(pos.x / Metric.gridUnitSide);
        //越界检查
        if (row < 0 || row >= currentMapData.mapHeight || column < 0 || column >= currentMapData.mapWidth)
            return null;
        return gridUnits[row, column];
    }

    [HideInInspector] public GridType selectType = GridType.None;


    public GridUnit last = null;
    //通过点击设置网格类型
    private void SetGridType(GridType selectType)
    {
        GridUnit clicked = SelectGrid();
        if (clicked&&last!=clicked)
        {
            clicked.gridUnitData.gridType = selectType;
            if (selectType == GridType.From)
            {
                if(from!=null)
                    from.gridUnitData.gridType = GridType.None;
                from = clicked;
            }
            else if (selectType == GridType.To)
            {
                if(to!=null)
                    to.gridUnitData.gridType = GridType.None;
                to = clicked;
            }
            Refresh();
            last = clicked;
        }
    }

    private GridUnit from, to;

    //导航测试
    public void NavigateTest()
    {
        List<GridUnitData> path, searched;
        Navigator.Instance.Navigate(this, from.gridUnitData, to.gridUnitData, out path, out searched);

        from.Refresh();
        to.Refresh();

        /*
        for (int i = 1; i < path.Count-1; ++i)
        {
            gridUnits[path[i].row, path[i].column].SetColor(Color.yellow);
        }
        */
    }

    //刷新整个地图
    private void Refresh()
    {
        for (int i = 0; i < currentMapData.mapHeight; ++i)
        {
            for (int j = 0; j < currentMapData.mapWidth; ++j)
            {
                gridUnits[i, j].Refresh();
            }
        }
    }

    private void Update()
    {
        SetGridType(selectType);
    }
}
