using System;
using System.Linq;
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
        _gridLayoutGroup.constraintCount = _gridHeight;
        _numberBomb = _bombPercent * (_gridHeight * _gridWidth) / 100;
        UIManager.Instance.SwitchGridOn();
        TilesCreate();
        BombsCreate();
        CountNeighbourBombs();
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
        UIManager.Instance.SwitchEndGameOff();
        UIManager.Instance.SwitchMenuOn();
    }

    private void TilesCreate()
    {
        for (int x = 0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                Tile tile = Instantiate(_tile, _gridTransform);
                tile.transform.localScale = Vector3.one;
                Vector2Int tilePos = new Vector2Int
                {
                    y = y,
                    x = x
                };
                tile.TilePos = tilePos;
                tile.TileClick += Tile_TileClick;
                tile.BombDetonation += Tile_BombDetonation;
                _tileArray[y, x] = tile;
            }
        }
    }

    private void Tile_BombDetonation(object sender, EventArgs e)
    {
        UIManager.Instance.SwitchGridOff();
        UIManager.Instance.SwitchEndGameOn();
    }

    private void Tile_TileClick(object sender, EventArgs e)
    {
        Tile oneTile = sender as Tile;
        if(oneTile.NumberNeighborBomb == 0 && !oneTile.IsBomb)
        {
            foreach (Vector2Int tilePos in FindAllNeighbourClosedTiles(oneTile.TilePos))
            {
                _tileArray[tilePos.y, tilePos.x].OpenTile();
            }
        }
    }

    private void BombsCreate()
    {
        List<Vector2Int> bombArrayList = new List<Vector2Int>();
        while (bombArrayList.Count < _numberBomb)
        {
            Vector2Int bombPoint = new Vector2Int
            {
                y = _random.Next(_gridHeight),
                x = _random.Next(_gridWidth)
            };

            if (!bombArrayList.Contains(bombPoint))
            {
                bombArrayList.Add(bombPoint);
            }
        }
        foreach (Vector2Int vector in bombArrayList)
        {
            _tileArray[vector.y, vector.x].IsBomb = true;
        }
    }

    private void CountNeighbourBombs()
    {
        for (int x = 0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                int count = 0;
                Vector2Int tilePos = new Vector2Int
                {
                    x = x,
                    y = y
                };
                foreach (Vector2Int vector in FindNeighbourTiles(tilePos))
                {
                    if (_tileArray[vector.y, vector.x].IsBomb)
                    {
                        count = count + 1;
                    }
                }
                _tileArray[y, x].NumberNeighborBomb = count;
            }
        }
    }

    private IEnumerable<Vector2Int> FindAllNeighbourClosedTiles(Vector2Int tilePos)
    {
        foreach (Vector2Int vector in FindNeighbourTiles(tilePos))
        {
            if (!_tileArray[vector.y, vector.x].IsOpen && !_tileArray[vector.y, vector.x].IsBomb && _tileArray[vector.y, vector.x].NumberNeighborBomb == 0)
            {
                yield return vector;
                foreach (Vector2Int newVector in FindAllNeighbourClosedTiles(vector))
                {
                    if (_tileArray[newVector.y, newVector.x].IsOpen)
                    {
                        yield break;
                    }
                    yield return newVector;
                    _tileArray[newVector.y, newVector.x].IsOpen = true;
                }
                _tileArray[vector.y, vector.x].IsOpen = true;
            }
            if (!_tileArray[vector.y, vector.x].IsOpen && !_tileArray[vector.y, vector.x].IsBomb && _tileArray[vector.y, vector.x].NumberNeighborBomb != 0)
            {
                yield return vector;
                _tileArray[vector.y, vector.x].IsOpen = true;
            }
        }
    }

    private IEnumerable<Vector2Int> FindNeighbourTiles(Vector2Int tilePos)
    {
        for (int x = tilePos.x - 1; x <= tilePos.x + 1; x++)
        {
            for (int y = tilePos.y - 1; y <= tilePos.y + 1; y++)
            {
                if (y < _gridHeight && x < _gridWidth && y >= 0 && x >= 0 && !(x == tilePos.x && y == tilePos.y))
                {
                    Vector2Int newTilePos = new Vector2Int
                    {
                        x = x,
                        y = y
                    };
                    yield return newTilePos;
                }
            }
        }
    }

    #endregion
}
