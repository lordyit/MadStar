using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planeta : MonoBehaviour
{

    public int _minhaPontuacao;

    public string _meuComportamento;

    public GameObject _meuOrbital;

    public Transform[] _pontos;

    bool _move = false;
    // Start is called before the first frame update
    void Start()
    {
        if (_meuComportamento == "planeta3")
        {
            _pontos[0].SetParent(null);
            _pontos[1].SetParent(null);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "move" && _meuComportamento == "planeta3")
        {
            _move = !_move;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_meuComportamento != "planeta4")
        {
            transform.Rotate(0, 0, 100 * Time.deltaTime);
        }
       

        if (_meuComportamento == "planeta5")
        {
            transform.SetParent(null);
            if (_meuOrbital != null)
            transform.RotateAround(_meuOrbital.transform.position, Vector3.forward, 40 * Time.deltaTime);
        }

        if (_meuComportamento == "planeta3")
        {
            if (_move)
            {
                transform.position = Vector3.MoveTowards(transform.position, _pontos[0].position, 2 * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _pontos[1].position, 2 * Time.deltaTime);
            }
        }

        if (_meuComportamento == "planeta4")
        {
            transform.Translate(-2 * Time.deltaTime, 0, 0);
        }
    }
}
