using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PhotonPlayer : MonoBehaviour
{

    private PhotonView _pv;

    public GameObject _myAvatar;

    public int _myNumber;
    // Start is called before the first frame update
    void Start()
    {
        _pv = GetComponent<PhotonView>();
        
        if (_pv.IsMine)
        {
            Invoke("Spawn", 0.1f);
        }
    }

    void Spawn()
    {
        Debug.Log("start Photonplayer");
        _myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"),
                                              GameSetup._gs._spawnPoints[Room._room._myNumberInRoom - 1].position,
                                              GameSetup._gs._spawnPoints[Room._room._myNumberInRoom - 1].rotation, 0);


        _myNumber = Room._room._myNumberInRoom;
    }

    // Update is called once per frame 
    void Update()
    {
        
    }
}
