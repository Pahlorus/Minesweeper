using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    #region Fields
    private int _numberRow = 10;
    private int _numberColumn = 10;
    private int _bombPercent = 10;
    private int _numberBomb;
    private Transform[,] _tileArray;
    private System.Random _random = new System.Random();
    #endregion

    #region Fields Initialized in Unity
    [SerializeField]
    private GameObject _grid;
    [SerializeField]
    private Transform _tilePref;

    #endregion

    #region Properties

    #endregion

    #region Unity Metods

    void Awake()
    {
        _grid.GetComponent<GridLayoutGroup>().constraintCount = _numberRow;
        _numberBomb = _bombPercent * (_numberRow * _numberColumn) / 100;
        _tileArray = new Transform[_numberRow, _numberColumn];
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
        for(int x =0; x < _numberColumn; x++)
        {
            for (int y = 0; y < _numberRow; y++)
            {
                Transform tile = Instantiate(_tilePref, _grid.transform);
                tile.GetChild(0).GetComponent<Text>().enabled = false;
                _tileArray[y, x] = tile;
            }
        }
    }
    public void BombsCreate()
    {
        for (int i = 0; i< _numberBomb; i++ )
        {
            int y = _random.Next(_numberRow);
            int x = _random.Next(_numberColumn);
            _tileArray[y, x].GetComponent<Tile>().IsBomb = true;
        }
    }
    public void CountNeighborBombCreate()
    {
        
        for (int x = 0; x < _numberColumn; x++)
        {
            for (int y = 0; y < _numberColumn; y++)
            {
                int count = 0;
                for (int x1 = x-1; x1<= x+1; x1++)
                {
                    for (int y1 = y-1; y1<= y+1; y1++)
                    {
                        if (y1 < _numberRow && x1 < _numberColumn && y1 >= 0 && x1>=0 && !(x1==x && y1==y))
                        {
                            if(_tileArray[y1, x1].GetComponent<Tile>().IsBomb)
                            {
                                count = count + 1;
                            }
                        }
                    }
                }
                if (count!=0)
                {
                    _tileArray[y, x].GetChild(0).GetComponent<Text>().text = count.ToString();
                }
                else
                {
                    _tileArray[y, x].GetChild(0).GetComponent<Text>().text = string.Empty;
                }
            }
        }


    }

    #endregion
}
