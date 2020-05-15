using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivity;
    public Rigidbody playerRb;
    public Transform cameraHolder;
    public Vector2 camClamp;

    public float cameraDistance;
    public float interpTime;

    private Vector2 rotation;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        PlayerCameraInput();
        //FollowPlayer();

    }

    private void PlayerCameraInput()
    {
        float x = Input.GetAxis("Mouse X") * sensitivity;
        float y = Input.GetAxis("Mouse Y") * sensitivity;

        rotation.x += x;

        rotation.y -= y;
        rotation.y = Mathf.Clamp(rotation.y, camClamp.x, camClamp.y);

        cameraHolder.rotation = Quaternion.Euler(rotation.y, rotation.x, 0);
        playerRb.rotation = Quaternion.Euler(0, rotation.x, 0);

        transform.LookAt(cameraHolder);
    }

    private void FollowPlayer() {
        float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(playerRb.position.x, playerRb.position.z));
        if (distance > cameraDistance || distance < cameraDistance) {
            Vector3 newPos = (-playerRb.transform.forward * cameraDistance + playerRb.position);
            newPos.y = playerRb.position.y + 2;
            transform.position = newPos;
        }
    }

}
