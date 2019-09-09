using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSMManager : MonoBehaviour
{
    public PlayerController controller;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }
}
