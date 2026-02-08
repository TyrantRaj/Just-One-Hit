using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float dodgeDistance = 4f;
    public float dodgeDuration = 0.25f;

    CharacterController controller;
    Animator animator;
    PlayerCombat combat;

    Vector2 moveInput;
    bool isRunning;

    float dodgeTimer;
    Vector3 dodgeDirection;

    bool IsAttacking => animator.GetBool("IsAttacking");
    bool IsDodging => animator.GetBool("IsDodging");

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        combat = GetComponent<PlayerCombat>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (IsAttacking || IsDodging) return;
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        isRunning = context.ReadValueAsButton();
    }

    void Update()
    {
        if (IsDodging)
        {
            if (dodgeTimer <= 0f)
            {
                dodgeTimer = dodgeDuration;
                dodgeDirection = -transform.forward;
            }

            dodgeTimer -= Time.deltaTime;
            controller.Move(dodgeDirection * (dodgeDistance / dodgeDuration) * Time.deltaTime);
            animator.SetFloat("MoveZ", 0);
            transform.rotation = combat.lockedRotation;
            return;
        }

        dodgeTimer = 0f;

        if (IsAttacking)
        {
            moveInput = Vector2.zero;
            animator.SetFloat("MoveZ", 0);
            transform.rotation = combat.lockedRotation;
            return;
        }

        float animMoveZ = 0f;
        bool forward = moveInput.y > 0f;

        if (forward)
            animMoveZ = isRunning ? 2f : 1f;
        else if (moveInput.y < 0f)
            animMoveZ = -1f;
        else if(moveInput.x < 0f || moveInput.x >0f)
            animMoveZ = 1f;

        animator.SetFloat("MoveZ", animMoveZ);

        float speed = (isRunning && forward) ? runSpeed : walkSpeed;

        Transform cam = Camera.main.transform;

        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = 0;
        camRight.y = 0;

        Vector3 moveDir =
            camForward.normalized * moveInput.y +
            camRight.normalized * moveInput.x;

        if (moveDir.sqrMagnitude > 0.01f)
        {
            controller.Move(moveDir * speed * Time.deltaTime);
        }
    }
}
