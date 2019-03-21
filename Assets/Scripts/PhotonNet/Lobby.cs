using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Lobby : MonoBehaviourPunCallbacks
{

    public static Lobby _lobby;

    public GameObject _battleButton;
    public GameObject _cancelBattleButton;
    public GameObject _info;

    private void Awake()
    {
        _lobby = this; //Cria o singleTon
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // Conecta ao master server Photon
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player Conectado ao master server Photon");
        PhotonNetwork.AutomaticallySyncScene = true;
        _battleButton.SetActive(true);
    }

    public void OnBattleButton()
    {
        _battleButton.SetActive(false);
        PhotonNetwork.JoinRandomRoom();
        _info.SetActive(true);
        _cancelBattleButton.SetActive(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Não conseguiu se conectar a uma sala, não deve haver salas disponíveis");
        CreateRoom();
    }

    void CreateRoom()
    {
        int _roomName = Random.Range(0, 10000);
        RoomOptions _roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSettings._multiplayerSettings._maxPlayer };
        PhotonNetwork.CreateRoom("Sala " + _roomName, _roomOps);
    }

    

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("falhou em criar sala");
        CreateRoom();
    }

    public void OnCancelButton()
    {
        _cancelBattleButton.SetActive(false);
        _battleButton.SetActive(true);
        _info.SetActive(false);
        PhotonNetwork.LeaveRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
