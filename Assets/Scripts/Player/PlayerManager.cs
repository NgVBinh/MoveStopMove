using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public Player player { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance );
        }else
        {
            instance = this;
        }
    }

    private void Start()
    {
        player = GetComponentInChildren<Player>();
    }

}
