using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controle : MonoBehaviour
{

    public GameObject _records;
    public GameObject _credits;

    bool _recodsB = false;
    bool _creditsB = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Jogar()
    {
        SceneManager.LoadScene(1);
    }

    public void Credits()
    {
        _creditsB = !_creditsB;
        _credits.SetActive(_creditsB);
    }

    public void Records()
    {
        _recodsB = !_recodsB;
        _records.SetActive(_recodsB);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
