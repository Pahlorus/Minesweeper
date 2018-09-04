using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    #region Fields
    private Sprite _underTexture;
    private int _numberBomb;
    private bool _isBomb;
    #endregion

    #region Fields Initialized in Unity
    [SerializeField]
    #endregion

    public Tile(bool isBomb )
    {
        _isBomb = isBomb;
    }

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

    #endregion
}
