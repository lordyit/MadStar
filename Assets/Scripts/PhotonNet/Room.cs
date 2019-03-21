using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine.UI;

public class Room : MonoBehaviourPunCallbacks, IInRoomCallbacks
{

    public Text _playersConnectedText;
    public Text _timeLeftText;
    public Text _nickName;

    public static Room _room;
    private PhotonView _pv;

    public bool _gameLoaded;

    public int _currentScene;

    // Player infos
    Photon.Realtime.Player[] _photonPlayers;

    public int _playerInRoom;
    public int _myNumberInRoom;
    public int _playerInGame;

    // Delayed start
    private bool _readyToCount;
    private bool _readyToStart;

    public float _startingTime;
    private float _lessThanMaxPlayers;
    private float _atMaxPlayers;
    private float _timeToStart;

    private void Awake()
    {
        if (Room._room == null)
        {
            Room._room = this;
        }
        else
        {
            if (Room._room != this)
            {
                Destroy(Room._room.gameObject);
                Room._room = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    // Start is called before the first frame update
    void Start()
    {
        _pv = GetComponent<PhotonView>();
        _readyToCount = false;
        _readyToStart = false;
        _lessThanMaxPlayers = _startingTime;
        _atMaxPlayers = 6;
        _timeToStart = _startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (MultiplayerSettings._multiplayerSettings._delayStart)
        {
            if (_playerInRoom == 1)
            {
                RestartTimer();
            }
            if (!_gameLoaded)
            {
                if (_readyToStart)
                {
                    _atMaxPlayers -= Time.deltaTime;
                    _lessThanMaxPlayers = _atMaxPlayers;
                    _timeToStart = _atMaxPlayers;
                }
                else if (_readyToCount)
                {
                    _lessThanMaxPlayers -= Time.deltaTime;
                    _timeToStart = _lessThanMaxPlayers;
                    if (_timeToStart < 0)
                    {
                        _timeToStart = 0;
                    }
                    _timeLeftText.text = "Time to Start: " + _timeToStart.ToString("F1");
                }
                Debug.Log("Tempo para começar " + _timeToStart);
                if (_timeToStart <= 0)
                {
                    StartGame();
                }
            }
        }
    }

    public void DelayStart()
    {
        Debug.Log("Mensagem para mostrar que tem " + _playerInRoom + " Players do no máxmo " + MultiplayerSettings._multiplayerSettings._maxPlayer);

        if (_playerInRoom > 1)
        {
            _readyToCount = true;
            _playersConnectedText.text = "Players Connected: " + _playerInRoom + "/" + MultiplayerSettings._multiplayerSettings._maxPlayer;
        }
        if (_playerInRoom == MultiplayerSettings._multiplayerSettings._maxPlayer)
        {
            _readyToStart = true;
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Entrou na sala");
        _photonPlayers = PhotonNetwork.PlayerList;
        _playerInRoom = _photonPlayers.Length;
        _myNumberInRoom = _playerInRoom;
        if (_nickName.text == "")
        {
            _nickName.text = "MadStar";
        }
        PhotonNetwork.NickName = _nickName.text;

        if (MultiplayerSettings._multiplayerSettings._delayStart)
        {
            DelayStart();
        }
        else
        {
            StartGame();
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("Novo player entrou");
        _photonPlayers = PhotonNetwork.PlayerList;
        _playerInRoom++;

        if (MultiplayerSettings._multiplayerSettings._delayStart)
        {
            DelayStart();
        }
    }

    void StartGame()
    {
        _gameLoaded = true;
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if (MultiplayerSettings._multiplayerSettings._delayStart)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        PhotonNetwork.LoadLevel(MultiplayerSettings._multiplayerSettings._multiplayerScene);
    }

    void RestartTimer()
    {
        _lessThanMaxPlayers = _startingTime;
        _timeToStart = _startingTime;
        _atMaxPlayers = 6;
        _readyToCount = false;
        _readyToStart = false;
    }

    void OnSceneFinishedLoading(Scene _scene, LoadSceneMode _mode)
    {
        _currentScene = _scene.buildIndex;
        if (_currentScene == MultiplayerSettings._multiplayerSettings._multiplayerScene)
        {
            _gameLoaded = true;
            if (MultiplayerSettings._multiplayerSettings._delayStart)
            {
                _pv.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
            }
            else
            {
                RPC_CreatePlayer();
            }
        }
    }

    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        _playerInGame++;
        if (_playerInGame == PhotonNetwork.PlayerList.Length)
        {
            _pv.RPC("RPC_CreatePlayer", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), new Vector3(1,1,1), Quaternion.identity, 0);
    }

}
