using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerMultiplayer : MonoBehaviour
{
    float _spd = 5;
    float _touchSpd = 60;
    float _shrink = 0.3f;


    public int _planetsCount;
    int _neutronCount = 0;

    bool _neutron = false;
    bool _gameOver = false;

    PhotonView _pv;
    
    Vector3 _touchPos;

    public GameObject[] _estado; // 0 para normal e 1 para neutron
    public GameObject _telaFinal;
    public GameObject _myCanvas;
    public GameObject[] _players;

    public Camera _cam;

    public Text _planets;
    public Text _finalMsg;
    public Text _nickName;

    // Start is called before the first frame update
    void Start()
    {
        _pv = GetComponent<PhotonView>();

        

        _touchPos = transform.position;
        if (!_pv.IsMine)
        {
            Destroy(_myCanvas);
            _nickName.text = _pv.Owner.NickName;
        }
        else
        {
            _nickName.text = PhotonNetwork.LocalPlayer.NickName;
            _myCanvas.transform.SetParent(null);
            _players = GameObject.FindGameObjectsWithTag("Player");
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "planeta")
        {
            _planetsCount += 1;
            _planets.text = _planetsCount.ToString();
            transform.localScale += new Vector3(0.01f, 0.01f,0.01f);
            col.gameObject.GetComponent<Coletavel>().CallDestroy();
        }

        if (col.gameObject.tag == "neutron")
        {
            _pv.RPC("RPC_Neutron", RpcTarget.All);
            col.gameObject.GetComponent<Coletavel>().CallDestroy();
            _neutronCount += 1;
            Invoke("StopNeutron", 7);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player" && _neutron)
        {
            if (!col.gameObject.GetComponent<PlayerMultiplayer>()._neutron)
            {
                col.gameObject.GetComponent<PlayerMultiplayer>().CallGetHit();
                if (_pv.IsMine)
                {
                    Kill();
                    Destroy(col.gameObject);
                }
            }
        }
        if (col.gameObject.tag == "limite")
        {
            GetComponent<Rigidbody2D>().velocity *= -1;
            Debug.Log("oi");
        }
    }

    void Kill()
    {
        GameSetup._gs._jogadores -= 1;
        Debug.Log(GameSetup._gs._jogadores);
        if (GameSetup._gs._jogadores == 1)
        {
            Winner();
        }
    }

    void Winner()
    {
        if (_pv.IsMine)
        {
            _gameOver = true;
            _telaFinal.SetActive(true);
            _finalMsg.text = "You Won!";
        }
    }

    void StopNeutron()
    {
        if (_neutronCount > 1)
        {
            _neutronCount -= 1;
            return;
        }
        _neutronCount -= 1;
        _pv.RPC("RPC_StopNeutron", RpcTarget.All);
    }

    [PunRPC]
    void RPC_StopNeutron()
    {
        _neutron = false;
        _estado[0].SetActive(true);
        _estado[1].SetActive(false);
    }

    [PunRPC]
    void RPC_Neutron()
    {
        _neutron = true;
        _estado[0].SetActive(false);
        _estado[1].SetActive(true);
    }

    public void CallGetHit()
    {
        _pv.RPC("RPC_GetHit", RpcTarget.All);
    }

    [PunRPC]
    void RPC_GetHit()
    {
        GameOver();
        Destroy(this.gameObject);
    }

    void FollowFinger()
    {
        if (Input.touchCount == 1 || Input.GetMouseButton(0))
        {
             _touchPos = (Vector2)_cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _cam.nearClipPlane));
        }
    }

    void GameOver()
    {
        if (_pv.IsMine)
        {
            _gameOver = true;
            _estado[0].SetActive(false);
            _telaFinal.SetActive(true);
            _finalMsg.text = "You Lost!";
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        if (!_gameOver && _pv.IsMine)
        {
            //transform.Translate(_spd * Time.deltaTime, 0, 0);
            FollowFinger();
            GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(transform.position, _touchPos, _touchSpd * Time.deltaTime));
        }
    }
}
