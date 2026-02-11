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
    Vector3 dodgeDir;

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
                dodgeDir = -transform.forward;
            }

            dodgeTimer -= Time.deltaTime;
            controller.Move(dodgeDir * (dodgeDistance / dodgeDuration) * Time.deltaTime);
            transform.rotation = combat.lockedRotation;
            animator.SetFloat("MoveZ", 0);
            return;
        }

        dodgeTimer = 0f;

        if (IsAttacking)
        {
            transform.rotation = combat.lockedRotation;
            animator.SetFloat("MoveZ", 0);
            return;
        }

        bool forward = moveInput.y > 0f;
        float animZ = 0f;

        if (forward)
            animZ = isRunning ? 2f : 1f;
        else if (moveInput.y < 0f)
            animZ = -1f;
        else if (Mathf.Abs(moveInput.x) > 0f)
            animZ = 1f;

        animator.SetFloat("MoveZ", animZ);

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
            controller.Move(moveDir * speed * Time.deltaTime);
    }
}
