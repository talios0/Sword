using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform player;

    private void FixedUpdate()
    {
        if (player.position.y < transform.position.y) player.transform.position = new Vector3(0, -1.015254f, 0);
    }

}
