using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class SpawnerMultiplayer : MonoBehaviour
{

    PhotonView _pv;

    // Start is called before the first frame update
    private void OnEnable()
    {
        _pv = GetComponent<PhotonView>();
        FirstSpawns();
        StartCoroutine("SpawnPlanets");
        StartCoroutine("SpawnNeutrons");
    }

    void FirstSpawns()
    {
        for (int i = 0; i < 300; i++)
        {
            float _randomX = Random.Range(-50, 50);
            float _randomY = Random.Range(-50, 50);

            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Planeta"), new Vector2(_randomX, _randomY), Quaternion.identity, 0);
        }

        for (int i = 0; i < 50; i++)
        {
            float _randomX = Random.Range(-50, 50);
            float _randomY = Random.Range(-50, 50);

            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Neutron"), new Vector2(_randomX, _randomY), Quaternion.identity, 0);
        }
    }

    IEnumerator SpawnPlanets()
    {
        while (true)
        {
            float _randomX = Random.Range(-50, 50);
            float _randomY = Random.Range(-50, 50);
            yield return new WaitForSeconds(0.1f);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Planeta"), new Vector2(_randomX, _randomY), Quaternion.identity, 0);
        }
    }

    IEnumerator SpawnNeutrons()
    {
        while (true)
        {
            float _randomX = Random.Range(-50, 50);
            float _randomY = Random.Range(-50, 50);
            yield return new WaitForSeconds(1f);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Neutron"), new Vector2(_randomX, _randomY), Quaternion.identity, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
