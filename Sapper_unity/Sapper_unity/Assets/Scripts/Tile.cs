using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class Tile : MonoBehaviour
{
    #region Fields
    private int _numberNeighborBomb;
    private bool _isBomb;
    private int[] _cell;
    #endregion

    #region Fields Initialized in Unity
    [SerializeField]
    private Image _tileImage;
    [SerializeField]
    private Text _tileTextNumberBomb;
    [SerializeField]
    private Sprite _underTexture;
    [SerializeField]
    private Sprite _underWithBombTexture;
    #endregion

    #region Properties
    public bool IsBomb
    {
        get { return _isBomb; }
        set { _isBomb = value; }
    }

    public int[] Cell
    {
        get { return _cell; }
        set { _cell = value; }
    }

    public int NumberNeighborBomb
    {
        get { return _numberNeighborBomb; }
        set
        {
            if(value != 0)
            {
                _tileTextNumberBomb.text = value.ToString();
                switch (value)
                {
                    case 1:
                        _tileTextNumberBomb.color = Color.white;
                        break;
                    case 2:
                        _tileTextNumberBomb.color = new Color32(49, 97, 236, 255);
                        break;
                    case 3:
                        _tileTextNumberBomb.color = new Color32(25, 150, 62, 255);
                        break;
                    case 4:
                        _tileTextNumberBomb.color = new Color32(186, 27, 57, 255);
                        break;
                    default:
                        _tileTextNumberBomb.color = Color.black;
                        break;
                }
            }
            else
            {
                _tileTextNumberBomb.text = string.Empty;
            }
            _numberNeighborBomb = value;
        }
    }

    #endregion

    #region Events
    public event EventHandler TileClick;
    public event EventHandler BombDetonation;
    #endregion

    #region Unity Metods
    private void Awake()
    {
        _tileTextNumberBomb.enabled = false;
        _cell = new int[2];
    }

    #endregion

    #region Metods
    public void OnClick()
    {
        OpenTile();
        TileClick(this, EventArgs.Empty);
    }

    public void OpenTile()
    {
        if (_isBomb)
        {
            _tileImage.sprite = _underWithBombTexture;
            BombDetonation(this, EventArgs.Empty);
        }
        else
        {
            _tileImage.sprite = _underTexture;
            _tileTextNumberBomb.enabled = true;
        }
    }
    #endregion

}
