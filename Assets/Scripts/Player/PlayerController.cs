using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed;
    public float movementDampen;
    public float runModifier;
    public float jumpForce;

    [Header("Checks")]
    public Transform groundTransform;
    public LayerMask groundLayers;

    private PlayerMoveState moveState;
    private PlayerAttackState attackState;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        UpdateGround();
        Move();
        Jump();
    }

    private void Move()
    {
        PlayerInput input = GetInput();
        Debug.Log(transform.forward);
        Vector3 movement = input.x * movementSpeed * transform.forward + new Vector3(0, 0, input.y * movementSpeed);
        if (input.x == 0) movement.x = MovementDampen(rb.velocity.x);
        if (input.y == 0) movement.z = MovementDampen(rb.velocity.z);
        rb.AddForce(movement, ForceMode.VelocityChange);
    }

    private float MovementDampen(float velocity)
    {
        return velocity * movementDampen * -1;
    }

    private void Jump()
    {
        if (moveState == PlayerMoveState.AIR || attackState == PlayerAttackState.SLASH) return;
        PlayerInput input = GetInput();
        if (!input.jump) return;
        rb.AddForce(jumpForce * transform.up, ForceMode.Impulse);
    }

    private void UpdateGround()
    {
        if (Physics.CheckSphere(groundTransform.position, 0.05f, groundLayers))
        {
            switch (moveState)
            {
                case PlayerMoveState.IDLE:
                    break;
                case PlayerMoveState.RUN:
                    break;
                case PlayerMoveState.AIR:
                    moveState = PlayerMoveState.IDLE;
                    break;
            }
        }
        else
        {
            moveState = PlayerMoveState.AIR;
        }
    }

    private PlayerInput GetInput()
    {
        PlayerInput p = new PlayerInput();
        p.x = (int)Input.GetAxisRaw("Horizontal");
        p.y = (int)Input.GetAxisRaw("Vertical");
        p.jump = false;
        if (Input.GetAxisRaw("Jump") != 0) p.jump = true;
        return p;
    }
}
