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
            _tileTextNumberBomb.text = value.ToString();
            _numberNeighborBomb = value;
        }
    }

    public Text TextNumberBomb
    {
        get { return _tileTextNumberBomb; }
        set { _tileTextNumberBomb = value; }
    }

    #endregion

    #region Unity Metods
    private void Awake()
    {



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
