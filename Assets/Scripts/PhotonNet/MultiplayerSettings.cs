using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerSettings : MonoBehaviour
{
    public static MultiplayerSettings _multiplayerSettings;

    public bool _delayStart;

    public int _maxPlayer;
    public int _menuScene;
    public int _multiplayerScene;

    private void Awake()
    {
        if (MultiplayerSettings._multiplayerSettings == null)
        {
            MultiplayerSettings._multiplayerSettings = this;
        }
        else
        {
            if (MultiplayerSettings._multiplayerSettings != this)
            {
                Destroy(this.gameObject);
            }
        }

        DontDestroyOnLoad(this.gameObject);
    }
}