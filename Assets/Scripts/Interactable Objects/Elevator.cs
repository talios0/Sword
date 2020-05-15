using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElevatorState { 
    TOP,
    UP,
    DOWN,
    BOTTOM
}

public class Elevator : MonoBehaviour
{
    public float groundHeight;
    public float maxHeight;
    public float speed;
    public float waitTime;

    private float trackedTime;
    private ElevatorState state;

    private GameObject player;
    public bool playerActive = false;

    private void Start()
    {
        state = ElevatorState.BOTTOM;
        trackedTime = waitTime;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        switch (state)
        {
            case ElevatorState.TOP:
                if (trackedTime > 0)
                {
                    trackedTime -= Time.deltaTime;
                    return;
                }
                else if (trackedTime < 0) {
                    trackedTime = waitTime;
                    state = ElevatorState.DOWN;
                }
                break;
            case ElevatorState.BOTTOM:
                if (trackedTime > 0)
                {
                    trackedTime -= Time.deltaTime;
                    return;
                }
                else if (trackedTime < 0)
                {
                    trackedTime = waitTime;
                    state = ElevatorState.UP;
                }
                break;
            case ElevatorState.UP:
                if (playerActive && player.GetComponent<PlayerController>().GetMoveState() != PlayerMoveState.JUMP)
                    player.transform.Translate(Vector3.up * speed * Time.deltaTime);
                transform.Translate(Vector3.up * speed *Time.deltaTime);
                if (transform.position.y > maxHeight) {
                    state = ElevatorState.TOP;
                    transform.position = new Vector3(transform.position.x, maxHeight, transform.position.z);
                }
                break;
            case ElevatorState.DOWN:
                transform.Translate(Vector3.up * -speed * Time.deltaTime);
                if (playerActive && player.GetComponent<PlayerController>().GetMoveState() != PlayerMoveState.JUMP)
                    player.transform.Translate(Vector3.up * -speed * Time.deltaTime);
                if (transform.position.y < groundHeight)
                {
                    state = ElevatorState.BOTTOM;
                    transform.position = new Vector3(transform.position.x, groundHeight, transform.position.z);
                }
                break;
        }
        

    }


}
