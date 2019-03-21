using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Coletavel : MonoBehaviour
{

    PhotonView _pv;
    // Start is called before the first frame update
    void Start()
    {
        _pv = GetComponent<PhotonView>();
    }

    public void CallDestroy()
    {
        _pv.RPC("RPC_Destroy", RpcTarget.All);
    }

    [PunRPC]
    void RPC_Destroy()
    {
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "planeta")
        {
            transform.Rotate(0, 0, 80 * Time.deltaTime);
        }
    }
}
