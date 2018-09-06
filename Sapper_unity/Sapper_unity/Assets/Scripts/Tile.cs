using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Tile : MonoBehaviour
{
    #region Fields
    
    
    private int _numberNeighborBomb;
    private bool _isBomb;

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

    public int NumberNeighborBomb
    {
        set
        {
            if(value != 0)
            {
                _tileTextNumberBomb.text = value.ToString();
            }
            else
            {
                _tileTextNumberBomb.text = string.Empty;
            }
            _numberNeighborBomb = value;
        }
    }

    #endregion

    #region Unity Metods
    private void Awake()
    {
        _tileTextNumberBomb.enabled = false;
    }

    #endregion

    #region Metods
    public void OnClick()
    {
        if (_isBomb)
        {
            this._tileImage.sprite = _underWithBombTexture;
        }
        else
        {
            this._tileImage.sprite = _underTexture;
            this._tileTextNumberBomb.enabled = true;
        }
    }
    #endregion
}
