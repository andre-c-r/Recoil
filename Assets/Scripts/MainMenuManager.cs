using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private string LevelInicial;

    public void Jogar()
    {
        SceneManager.LoadScene(LevelInicial);

    }

    public void Sair()
    {
        Application.Quit();
    }

}
