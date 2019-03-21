using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quasar : MonoBehaviour
{

    ParticleSystem _pa;
    ParticleSystem.MainModule _ma;
    // Start is called before the first frame update
    void Start()
    {
        _pa = GetComponent<ParticleSystem>();
        _ma = _pa.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 101 * Time.deltaTime);
    }
}
