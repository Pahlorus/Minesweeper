using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Fields Initialized in Unity
    [SerializeField]
    private GameObject _menuScreen;
    [SerializeField]
    private GameObject _gridScreen;
    [SerializeField]
    private GameObject _endGameScreen;
    #endregion

    #region Properties
    public static UIManager Instance { get; private set; }
    #endregion

    #region Unity Metods

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    #endregion
}
