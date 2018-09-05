using System;
using System.Collections;
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
    private System.Random _random = new System.Random();
    #endregion

    #region Fields Initialized in Unity
    [SerializeField]
    private Transform _gridTransform;
    [SerializeField]
    private GridLayoutGroup _gridLayoutGroup;
    //[SerializeField]
    private Transform _tilePref;
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
        TilesCreate();
        BombsCreate();
        CountNeighborBombCreate();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region Metods
    public void TilesCreate()
    {
        for(int x =0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
               // Transform tile = Instantiate(_tilePref, _gridTransform);
                Tile tile = Instantiate(_tile, _gridTransform);
                tile.TextNumberBomb.enabled = false;
                _tileArray[y, x] = tile;
            }
        }
    }
    public void BombsCreate()
    {
        for (int i = 0; i< _numberBomb; i++ )
        {
            int y = _random.Next(_gridHeight);
            int x = _random.Next(_gridWidth);
            _tileArray[y, x].GetComponent<Tile>().IsBomb = true;
        }
    }
    public void CountNeighborBombCreate()
    {
        
        for (int x = 0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridWidth; y++)
            {
                int count = 0;
                for (int x1 = x-1; x1<= x+1; x1++)
                {
                    for (int y1 = y-1; y1<= y+1; y1++)
                    {
                        if (y1 < _gridHeight && x1 < _gridWidth && y1 >= 0 && x1>=0 && !(x1==x && y1==y))
                        {
                            if(_tileArray[y1, x1].GetComponent<Tile>().IsBomb)
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
