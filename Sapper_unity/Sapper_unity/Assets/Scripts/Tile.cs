using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Tile : MonoBehaviour
{
    [SerializeField]
    private Image _tileImage;
    [SerializeField]
    private Text _tileTextNumberBomb;
    [SerializeField]
    private Sprite _upperTexture;
    [SerializeField]
    private Sprite _upperTextureWithFlag;
    [SerializeField]
    private Sprite _underTexture;
    [SerializeField]
    private Sprite _underWithBombTexture;
    [SerializeField]
    private Color32[] _colors;

    private int _numberNeighborBomb;
    private bool _isBomb;
    private bool _isOpen;
    private Vector2Int _tilePos;

    public bool IsBomb
    {
        get { return _isBomb; }
        set { _isBomb = value; }
    }

    public bool IsOpen
    {
        get { return _isOpen; }
        private set { _isOpen = value; }
    }

    public Vector2Int TilePos
    {
        get { return _tilePos; }
        set { _tilePos = value; }
    }

    public int NumberNeighborBomb
    {
        get { return _numberNeighborBomb; }
        set
        {
            if(value != 0)
            {
                _tileTextNumberBomb.color = _colors[value - 1];
                _tileTextNumberBomb.text = value.ToString();
            }
            else
            {
                _tileTextNumberBomb.text = string.Empty;
            }
            _numberNeighborBomb = value;
        }
    }

    public event EventHandler TileClick;
    public event EventHandler BombDetonation;


    private void Awake()
    {
        _isOpen = false;
        _tileTextNumberBomb.enabled = false;
    }

    public void OnClick()
    {
        TileClick(this, EventArgs.Empty);
    }

    public  void OnPointerClick(BaseEventData eventData)
    {
        PointerEventData newEventData = (PointerEventData)eventData;
        if (newEventData.button == PointerEventData.InputButton.Right)
        {
            _tileImage.sprite = _upperTextureWithFlag;
        }
    }

    public void OpenTile()
    {
        if (_isBomb)
        {
            _tileImage.sprite = _underWithBombTexture;
            BombDetonation(this, EventArgs.Empty);
            _isOpen = true;
        }
        else
        {
            _tileImage.sprite = _underTexture;
            _tileTextNumberBomb.enabled = true;
            _isOpen = true;

        }
    }
}
