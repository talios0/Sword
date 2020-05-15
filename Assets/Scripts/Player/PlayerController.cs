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

    [Header("Ground Check")]
    public Transform groundTransform;
    public LayerMask groundLayers;
    public float groundHeight;

    [Header("Component References")]
    public Animator anim;
    public GameObject model;
    private Rigidbody rb;

    // --- Player States --- //
    private PlayerMoveState moveState;
    private PlayerAttackState attackState;
    private PlayerStandState standState;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        PlayerMovement();
        DampenMovement();
        Jump();
        UpdateGround();
    }

    private void LateUpdate()
    {
        UpdateAnimations();
    }

    private void PlayerMovement()
    {
        PlayerInput input = GetInput();
        if (moveState != PlayerMoveState.JUMP)
        {
            if (input.x != 0 || input.y != 0) moveState = PlayerMoveState.RUN;
            else moveState = PlayerMoveState.IDLE;
        }
        Vector3 movement = (input.y * movementSpeed * transform.forward);
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
        if (standState == PlayerStandState.AIR) return;
        PlayerInput input = GetInput();
        if (!input.jump) return;
        if (rb.velocity.y > 0) return;
        if (!Physics.Raycast(groundTransform.position, -transform.up, groundHeight)) return;
        rb.AddForce(jumpForce * transform.up, ForceMode.Impulse);
        moveState = PlayerMoveState.JUMP;
        anim.Play("Jump");
    }

    private void UpdateGround()
    {
        Vector3 colSize = model.GetComponent<BoxCollider>().size;
        bool touchingGround = Physics.CheckBox(groundTransform.position, new Vector3(colSize.x/2, groundHeight/2, colSize.z/2), transform.rotation, groundLayers);
        
        switch (moveState)
        {
            case PlayerMoveState.IDLE:
                if (!touchingGround) moveState = PlayerMoveState.FALLING;
                break;
            case PlayerMoveState.RUN:
                if (!touchingGround) moveState = PlayerMoveState.FALLING;
                break;
            case PlayerMoveState.JUMP:
                if (rb.velocity.y < 0) moveState = PlayerMoveState.FALLING;
                break;
            case PlayerMoveState.FALLING:
                if (touchingGround || rb.velocity.y >= 0) moveState = PlayerMoveState.IDLE;
                break;
        }

        switch (standState) {
            case PlayerStandState.GROUND:
                if (!touchingGround) standState = PlayerStandState.AIR;
                break;
            case PlayerStandState.AIR:
                if (touchingGround) standState = PlayerStandState.GROUND;
                break;
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
