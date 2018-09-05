using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Tile : MonoBehaviour
{
    #region Fields
    private Sprite _underTexture;
    private Sprite _tileTexture;
    private int _numberBomb;
   
    #endregion

    #region Fields Initialized in Unity
    [SerializeField]
    private bool _isBomb;
    #endregion

    #region Properties
    public bool IsBomb
    {
        get { return _isBomb; }
        set { _isBomb = value; }
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
            this.GetComponent<Image>().sprite = Resources.Load<Sprite>("TileUnderWithBomb");
        }
        else
        {
            this.GetComponent<Image>().sprite = Resources.Load<Sprite>("TileUnder");
        }
        
        
    }
    #endregion
}
