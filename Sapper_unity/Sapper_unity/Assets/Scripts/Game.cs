using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    #region Fields
    private int _gridHeight = 10;
    private int _gridWidth = 10;
    private int _bombPercent = 10;
    private int _numberBomb;
    private Tile[,] _tileArray;
    private List<int[]> _bombArrayList;
    private System.Random _random = new System.Random();
    #endregion

    #region Fields Initialized in Unity
    [SerializeField]
    private Transform _gridTransform;
    [SerializeField]
    private GridLayoutGroup _gridLayoutGroup;
    [SerializeField]
    private Tile _tile;

    #endregion

    #region Properties

    #endregion

    #region Unity Metods

    #endregion

    #region Metods
    public void StartGame()
    {
        _tileArray = new Tile[_gridHeight, _gridWidth];
        _bombArrayList = new List<int[]>();
        _gridLayoutGroup.constraintCount = _gridHeight;
        _numberBomb = _bombPercent * (_gridHeight * _gridWidth) / 100;
        UIManager.Instance.SwitchGridOn();
        TilesCreate();
        BombsCreate();
        CountNeighborBombCreate();
        UIManager.Instance.SwitchMenuOff();
    }

    public void StopGame()
    {
        for (int x= 0; x< _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                _tileArray[y, x].TileClick-= Tile_TileClick;
                _tileArray[y, x].BombDetonation -= Tile_BombDetonation;
                Destroy(_tileArray[y, x].gameObject);           
            }
        }
        //Array.Clear(_tileArray, 0, _tileArray.Length);
        _bombArrayList.Clear();
        UIManager.Instance.SwitchEndGameOff();
        UIManager.Instance.SwitchMenuOn();
    }

    public void TilesCreate()
    {
        for (int x = 0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                Tile tile = Instantiate(_tile, _gridTransform);
                tile.transform.localScale = Vector3.one;
                tile.Cell[0] = y;
                tile.Cell[1] = x;
                tile.TileClick += Tile_TileClick;
                tile.BombDetonation += Tile_BombDetonation;
                _tileArray[y, x] = tile;
            }
        }
    }

    private void Tile_BombDetonation(object sender, EventArgs e)
    {
        UIManager.Instance.SwitchEndGameOn();
        UIManager.Instance.SwitchGridOff();
    }

    private void Tile_TileClick(object sender, EventArgs e)
    {
        Tile oneTile = sender as Tile;

        if (oneTile.NumberNeighborBomb == 0 && !oneTile.IsBomb)
        {
            List<int[]> emptyCellList = new List<int[]>();
            List<int[]> textCellList = new List<int[]>();
            FindNeighborEmptyCell(oneTile.Cell[1], oneTile.Cell[0], emptyCellList);
            FindNeighborTextCell(emptyCellList, textCellList);
            foreach (int[] arr in emptyCellList)
            {
                _tileArray[arr[0], arr[1]].OpenTile();
            }
            foreach (int[] arr in textCellList)
            {
                _tileArray[arr[0], arr[1]].OpenTile();
            }
        }
    }

    public void BombsCreate()
    {
        while (_bombArrayList.Count < _numberBomb)
        {
            int[] _bombPoint = new int[2]; ;
            _bombPoint[0] = _random.Next(_gridHeight);
            _bombPoint[1] = _random.Next(_gridWidth);
            if (!IsListContainsArray(_bombArrayList,_bombPoint))
            {
                _bombArrayList.Add(_bombPoint);
            }
        }
        foreach (int[] arr in _bombArrayList)
        {
            _tileArray[arr[0], arr[1]].IsBomb = true;
        }

    }

    public void CountNeighborBombCreate()
    {

        for (int x = 0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                int count = 0;
                for (int x1 = x - 1; x1 <= x + 1; x1++)
                {
                    for (int y1 = y - 1; y1 <= y + 1; y1++)
                    {
                        if (y1 < _gridHeight && x1 < _gridWidth && y1 >= 0 && x1 >= 0 && !(x1 == x && y1 == y))
                        {
                            if (_tileArray[y1, x1].IsBomb)
                            {
                                count = count + 1;
                            }
                        }
                    }
                }
                _tileArray[y, x].NumberNeighborBomb = count;
            }
        }
    }

    public bool IsListContainsArray(List<int[]> list, int[] array)
    {
        bool isListContainsArray = false;
        for (int i = 0; i<list.Count; i++)
        {
            if (list[i].SequenceEqual(array))
            {
                isListContainsArray = true;
                break;
            }
        }
        return isListContainsArray;
    }

    public void FindNeighborEmptyCell(int x, int y, List<int[]> list)
    {
        bool isEmptyCell = false;
       // List<int[]> emptyCellList = list;
        
        for (int x1 = x - 1; x1 <= x + 1; x1++)
        {
            for (int y1 = y - 1; y1 <= y + 1; y1++)
            {
                if (y1 < _gridHeight && x1 < _gridWidth && y1 >= 0 && x1 >= 0 && !(x1 == x && y1 == y))
                {
                    int[] emptyCell = new int[2];
                    emptyCell[0] = y1;
                    emptyCell[1] = x1;

                    if (!_tileArray[y1, x1].IsBomb && _tileArray[y1, x1].NumberNeighborBomb == 0 && !IsListContainsArray(list, emptyCell))
                    {
                        list.Add(emptyCell);
                        isEmptyCell = true;
                    }
                }
            }
        }
        if (isEmptyCell)
        {
            for (int i =0; i< list.Count;i++)
            {
                FindNeighborEmptyCell(list[i][1], list[i][0], list);
            }
        }
    }

    public void FindNeighborTextCell(List<int[]> initislList, List<int[]> nextList)
    {
        for (int i = 0; i< initislList.Count; i++)
        {
            int x = initislList[i][1];
            int y = initislList[i][0];
            for (int x1 = x - 1; x1 <= x + 1; x1++)
            {
                for (int y1 = y - 1; y1 <= y + 1; y1++)
                {
                    if (y1 < _gridHeight && x1 < _gridWidth && y1 >= 0 && x1 >= 0 && !(x1 == x && y1 == y))
                    {
                        int[] emptyCell = new int[2];
                        emptyCell[0] = y1;
                        emptyCell[1] = x1;
                        if (!_tileArray[y1, x1].IsBomb && _tileArray[y1, x1].NumberNeighborBomb != 0 && !IsListContainsArray(nextList, emptyCell))
                        {
                            nextList.Add(emptyCell);
                        }
                    }
                }
            }
        }
    }
    #endregion
}
