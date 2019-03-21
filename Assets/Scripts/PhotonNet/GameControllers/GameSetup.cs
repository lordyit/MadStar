using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameSetup : MonoBehaviour
{

    public static GameSetup _gs;

    PhotonView _pv;

    public int _jogadores;


    public  List<Transform> _spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        _pv = GetComponent<PhotonView>();
        Invoke("Jogadores", 0.2f);
    }

    void Jogadores()
    {
        _jogadores = Room._room._playerInRoom;
    }

    public void CallPlayerDown()
    {
        _pv.RPC("RPC_PlayerDown", RpcTarget.All);
    }

    [PunRPC]
    void RPC_PlayerDown()
    {
        _jogadores -= 1;
    }

    private void OnEnable()
    {
        if (GameSetup._gs == null)
        {
            GameSetup._gs = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
