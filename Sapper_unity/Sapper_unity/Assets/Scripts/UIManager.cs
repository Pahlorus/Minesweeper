using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _menuScreen;
    [SerializeField]
    private GameObject _gridScreen;
    [SerializeField]
    private GameObject _endGameScreen;

    public static UIManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public void SwitchMenuOff()
    {
        _menuScreen.SetActive(false);
    }

    public void SwitchMenuOn()
    {
        _menuScreen.SetActive(true);
    }

    public void SwitchGridOff()
    {
        _gridScreen.SetActive(false);
    }

    public void SwitchGridOn()
    {
        _gridScreen.SetActive(true);
    }

    public void SwitchEndGameOn()
    {
        _endGameScreen.SetActive(true);
    }

    public void SwitchEndGameOff()
    {
        _endGameScreen.SetActive(false);
    }
}
