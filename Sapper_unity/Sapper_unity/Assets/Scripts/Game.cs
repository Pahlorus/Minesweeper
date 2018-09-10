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
                Vector2Int tilePos = new Vector2Int();
                tilePos.y = y;
                tilePos.x = x;
                tile.TilePos = tilePos;
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
            foreach (Vector2 tilePos in FindNeighborEmptyCell(oneTile.TilePos))
            {
                _tileArray[(int)tilePos.y, (int)tilePos.x].OpenTile();
            }
        }
    }

    private void BombsCreate()
    {
        List<Vector2> bombArrayList = new List<Vector2>();
        while (bombArrayList.Count < _numberBomb)
        {
            Vector2 bombPoint;
            bombPoint.y = _random.Next(_gridHeight);
            bombPoint.x = _random.Next(_gridWidth);
            if (!bombArrayList.Contains(bombPoint))
            {
                bombArrayList.Add(bombPoint);
            }
        }
        foreach (Vector2 vector in bombArrayList)
        {
            _tileArray[(int)vector.y, (int)vector.x].IsBomb = true;
        }
    }

    private void CountNeighborBombCreate()
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

    private IEnumerable<Vector2Int> FindNeighborEmptyCell(Vector2Int tilePos)
    {
        for (int x = tilePos.x - 1; x <= tilePos.x + 1; x++)
        {
            for (int y = tilePos.y - 1; y <= tilePos.y + 1; y++)
            {
                if (y < _gridHeight && x < _gridWidth && y >= 0 && x >= 0 && !(x == tilePos.x && y == tilePos.y))
                {

                    Vector2Int newTilePos = new Vector2Int();
                    newTilePos.x = x;
                    newTilePos.y = y;

                    if(!_tileArray[newTilePos.y, newTilePos.x].IsOpen && !_tileArray[newTilePos.y, newTilePos.x].IsBomb && _tileArray[newTilePos.y, newTilePos.x].NumberNeighborBomb == 0)
                    {
                        yield return newTilePos;
                        foreach (Vector2Int vector in FindNeighborEmptyCell(newTilePos))
                        {
                            if (_tileArray[vector.y, vector.x].IsOpen)
                            {
                                yield break;
                            }
                            yield return vector;
                            _tileArray[vector.y, vector.x].IsOpen = true;
                        }
                        _tileArray[newTilePos.y, newTilePos.x].IsOpen = true;
                    }

                    if (!_tileArray[newTilePos.y, newTilePos.x].IsOpen && !_tileArray[newTilePos.y, newTilePos.x].IsBomb && _tileArray[newTilePos.y, newTilePos.x].NumberNeighborBomb != 0)
                    {
                        yield return newTilePos;
                        _tileArray[newTilePos.y, newTilePos.x].IsOpen = true;
                    }
                }
            }
        }
    }

    #endregion
}
