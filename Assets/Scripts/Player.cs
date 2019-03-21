using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Player : MonoBehaviour
{

    float _spd = 5;
    float _touchSpd = 2;
    float _shrink = 0.3f;


    public int _lives = 3;
    public int _planetsCount;
    public int _spawns;

    bool _neutron = false;
    bool _gameOver = false;

    Touch _touch;

    Collider2D _touched;

    Vector3 _touchPos;
    Vector3 _distanceSpawn = new Vector3(62.9f,0,0);

    public GameObject _backGround;
    public GameObject[] _estado; // 0 para normal e 1 para neutron
    public GameObject _telaFinal;

    public Camera _cam;

    public Text _planets;
    public Text _planetsFinal;
    public Text _finalMsg;

    // Start is called before the first frame update
    void Start()
    {
        _spawns = 0;
        if (!PlayerPrefs.HasKey("ranking"))
        {
            PlayerPrefs.SetInt("ranking", 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "blackHole" && col != _touched)
        {
            if (!_neutron)
            {
                GetHit();
                _touched = col;
            }
            else
            {
                _touched = col;
                _planetsCount += 3;
                _planets.text = _planetsCount.ToString();
                Destroy(col.gameObject);
            }
        }

        if (col.gameObject.tag == "backGround")
        {
            GameObject _spawnar = Instantiate(_backGround, col.gameObject.transform.position + _distanceSpawn, Quaternion.identity);
            _spawns += 1;
            _spawnar.GetComponent<Spawner>()._spawn += 1;
        }

        if (col.gameObject.tag == "planeta")
        {
            _planetsCount += col.gameObject.GetComponent<Planeta>()._minhaPontuacao;
            _planets.text = _planetsCount.ToString();
            Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "neutron")
        {
            _neutron = true;
            _estado[0].SetActive(false);
            _estado[1].SetActive(true);
            Destroy(col.gameObject);
            Invoke("StopNeutron", 5);
        }
    }

    void StopNeutron()
    {
        _neutron = false;
        _estado[0].SetActive(true);
        _estado[1].SetActive(false);
    }

    void GetHit()
    {
        _lives -= 1;
        if (_lives < 1)
        {
            GameOver();
        }
        else
        {
            transform.localScale -= new Vector3(_shrink, _shrink, _shrink);
            _spd += 2;
            _touchSpd += 1;
        }
    }

    void FollowFinger()
    {
        if (Input.touchCount == 1 || Input.GetMouseButton(0))
        {
            _touchPos = (Vector2)_cam.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    void GameOver()
    {
        _gameOver = true;
        _estado[0].SetActive(false);
        _telaFinal.SetActive(true);
        _planetsFinal.text = _planets.text;
        ChecarRanking();
    }

    void ChecarRanking()
    {
        if (PlayerPrefs.HasKey("ranking"))
        {
            int _playerRecord = PlayerPrefs.GetInt("ranking");
            if (_planetsCount > _playerRecord)
            {
                _finalMsg.text = "You beat the record of " + _playerRecord + " planets consumed!";
                PlayerPrefs.SetInt("ranking", _planetsCount);
            }
            else
            {
                _finalMsg.text = "You don't beaten the record of " + _playerRecord + " planets consumed!";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gameOver)
        {
            transform.Translate(_spd * Time.deltaTime, 0, 0);
            FollowFinger();
            _touchPos = new Vector2(transform.position.x, _touchPos.y);
            transform.position = Vector3.Lerp(transform.position, _touchPos, _touchSpd * Time.deltaTime);
        }
    }
}
