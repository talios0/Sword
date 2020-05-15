using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;

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
    public Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        UpdateGround();
        PlayerMovement();
        DampenMovement();
        Jump();
    }

    private void LateUpdate()
    {
        UpdateAnimations();
    }

    private void PlayerMovement()
    {
        PlayerInput input = GetInput();
        if (moveState != PlayerMoveState.AIR)
        {
            if (input.x != 0 || input.y != 0) moveState = PlayerMoveState.RUN;
            else moveState = PlayerMoveState.IDLE;
        }
        Vector3 movement = input.y * movementSpeed * transform.forward;
        rb.AddForce(movement, ForceMode.VelocityChange);
    }

    private void DampenMovement()
    {
        Vector3 dampen = -rb.velocity * movementDampen;
        dampen.y = 0;
        rb.AddForce(dampen, ForceMode.VelocityChange);
    }

    private void Jump()
    {
        if (moveState == PlayerMoveState.JUMP || moveState == PlayerMoveState.AIR || attackState == PlayerAttackState.SLASH) return;
        PlayerInput input = GetInput();
        if (!input.jump) return;
        rb.AddForce(jumpForce * transform.up, ForceMode.Impulse);
        moveState = PlayerMoveState.JUMP;
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
                case PlayerMoveState.JUMP:
                    moveState = PlayerMoveState.IDLE;
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

    private void UpdateAnimations()
    {
        anim.SetInteger("MoveState", (int)moveState);
    }
}
