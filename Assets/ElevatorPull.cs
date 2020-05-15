using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPull : MonoBehaviour
{
    public Elevator main;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerModel")
        {
            main.playerActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "PlayerModel")
        {
            main.playerActive = false;
        }
    }
}
