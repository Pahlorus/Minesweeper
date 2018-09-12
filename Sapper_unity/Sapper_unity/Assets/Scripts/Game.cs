using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Game : MonoBehaviour
{
    [SerializeField]
    private Transform _gridTransform;
    [SerializeField]
    private GridLayoutGroup _gridLayoutGroup;
    [SerializeField]
    private Tile _tile;

    private int _gridHeight = 10;
    private int _gridWidth = 10;
    private int _bombPercent = 10;
    private int _numberBomb;
    private Tile[,] _tileArray;
    private System.Random _random = new System.Random();

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
        FindAndOpenTilesRecursively(oneTile);
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
                foreach (Tile tile in FindNeighbourTiles(_tileArray[y, x]))
                {
                    if (tile.IsBomb)
                    {
                        count = count + 1;
                    }
                }
                _tileArray[y, x].NumberNeighborBomb = count;
            }
        }
    }

    private void FindAndOpenTilesRecursively(Tile tile)
    {
        if (tile.IsOpen)
        {
            return;
        }
        tile.OpenTile();
        if (tile.IsBomb)
        {
            return;
        }
        if (tile.NumberNeighborBomb == 0)
        {
            foreach (Tile neighbourTile in FindNeighbourTiles(tile))
            {
                if (neighbourTile.NumberNeighborBomb == 0)
                {
                    FindAndOpenTilesRecursively(neighbourTile);
                }
                if (neighbourTile.NumberNeighborBomb != 0)
                {
                    neighbourTile.OpenTile();
                }
            }
        }
    }

    private IEnumerable<Tile> FindNeighbourTiles(Tile tile)
    {
        for (int x = tile.TilePos.x - 1; x <= tile.TilePos.x + 1; x++)
        {
            for (int y = tile.TilePos.y - 1; y <= tile.TilePos.y + 1; y++)
            {
                if (y < _gridHeight && x < _gridWidth && y >= 0 && x >= 0 && !(x == tile.TilePos.x && y == tile.TilePos.y))
                {
                    Vector2Int newTilePos = new Vector2Int
                    {
                        x = x,
                        y = y
                    };
                    yield return _tileArray[newTilePos.y, newTilePos.x];
                }
            }
        }
    }
}
