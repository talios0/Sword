using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivity;
    public Rigidbody playerRb;

    public float cameraDistance;
    public float interpTime;

    private float rotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        PlayerCameraInput();
        FollowPlayer();

    }

    private void PlayerCameraInput()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        rotation -= y * sensitivity;
        rotation = Mathf.Clamp(rotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(rotation, 0, 0);

        playerRb.rotation = Quaternion.Euler(playerRb.rotation.eulerAngles + x * sensitivity * Vector3.up);
    }

    private void FollowPlayer() {
        float distance = Mathf.Sqrt(Mathf.Pow(transform.position.x - playerRb.position.x, 2) + Mathf.Pow(transform.position.z - playerRb.position.z, 2));
        if (distance > cameraDistance) {
            Vector3 newPos = playerRb.position - transform.forward * cameraDistance;
            newPos.y = playerRb.position.y + 2;
            transform.position = Vector3.Lerp(transform.position, newPos, interpTime);
        }
    }

}
