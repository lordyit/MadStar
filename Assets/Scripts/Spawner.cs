using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject[] _planetas;
    public GameObject _neutronStars;
    public GameObject _blackHoles;

    float _downLimit = -4.5f;
    float _upLimit = 6.2f;
    float _xDireita = 30;
    float _xEsquerda = -20;

    public int _spawn = 0;

    public GameObject _player;

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    void Spawn()
    {

        if (_spawn == 0)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 _pos = new Vector2(Random.Range(20, 52), Random.Range(_downLimit, _upLimit));
                Instantiate(_blackHoles, _pos, Quaternion.identity);
            }
            for (int i = 0; i < 30; i++)
            {
                
                Vector2 _pos = new Vector2(Random.Range(20, 52), Random.Range(_downLimit, _upLimit));
                Instantiate(_planetas[(int)Random.Range(0,3.99f)], _pos, Quaternion.identity);
            }
            Vector2 _posNeutron = new Vector2(Random.Range(20, 52), Random.Range(_downLimit, _upLimit));
            Instantiate(_neutronStars, _posNeutron, Quaternion.identity);
        }
        else
        {
            for (int i = 0; i < 10 + (_spawn * 2); i++)
            {
                Vector2 _pos = new Vector2(transform.position.x + Random.Range(_xEsquerda, _xDireita), Random.Range(_downLimit, _upLimit));
                Instantiate(_blackHoles, _pos, Quaternion.identity);
            }
            for (int i = 0; i < 30; i++)
            {
                Vector2 _pos = new Vector2(transform.position.x + Random.Range(_xEsquerda, _xDireita), Random.Range(_downLimit, _upLimit));
                Instantiate(_planetas[(int)Random.Range(0, 3.99f)], _pos, Quaternion.identity);
            }

            int _temNeutron = (int)Random.Range(0, 1.99f);
            if (_temNeutron == 0)
            {
                Vector2 _posNeutron = new Vector2(transform.position.x + Random.Range(_xEsquerda, _xDireita), Random.Range(_downLimit, _upLimit));
                Instantiate(_neutronStars, _posNeutron, Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
