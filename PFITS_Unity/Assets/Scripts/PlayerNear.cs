using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNear : MonoBehaviour
{
    public bool isPlayerNear;
    public int range = 2;
    [HideInInspector] public GameObject player;
    [HideInInspector] public Vector3 playerTran;

    public void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    public void CalcDistance()
    {
        playerTran = player.transform.position;

        if ((Vector3.Distance(this.transform.position, player.transform.position) <= range))
            isPlayerNear = true;
        else
            isPlayerNear = false;
    }
}
