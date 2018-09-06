using System;
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
    private List <Array> _bombArrayList;
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

    void Awake()
    {
        _gridLayoutGroup.constraintCount = _gridHeight;
        _numberBomb = _bombPercent * (_gridHeight * _gridWidth) / 100;
        _tileArray = new Tile[_gridHeight, _gridWidth];
        _bombArrayList = new List<Array>();
        TilesCreate();
        BombsCreate();
        CountNeighborBombCreate();
    }
    #endregion

    #region Metods
    public void TilesCreate()
    {
        for(int x =0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                Tile tile = Instantiate(_tile, _gridTransform);
                tile.transform.localScale = Vector3.one;
                _tileArray[y, x] = tile;
            }
        }
    }
    public void BombsCreate()
    {
        while(_bombArrayList.Count < _numberBomb)
        {
             int[] _bombPoint = new int[2]; ;
            _bombPoint[0] = _random.Next(_gridHeight);
            _bombPoint[1] = _random.Next(_gridWidth);
            if (!_bombArrayList.Contains(_bombPoint))
            {
                _bombArrayList.Add(_bombPoint);
            }
        }
        foreach(int[] arr in _bombArrayList)
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
                for (int x1 = x-1; x1<= x+1; x1++)
                {
                    for (int y1 = y-1; y1<= y+1; y1++)
                    {
                        if (y1 < _gridHeight && x1 < _gridWidth && y1 >= 0 && x1>=0 && !(x1==x && y1==y))
                        {
                            if(_tileArray[y1, x1].IsBomb)
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
    #endregion
}
