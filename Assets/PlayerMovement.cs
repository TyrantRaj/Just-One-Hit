using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    CharacterController controller;
    Animator animator;
    PlayerCombat combat;
    Vector2 moveInput;

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

    void Update()
    {
        if (IsAttacking || IsDodging)
        {
            moveInput = Vector2.zero;
            animator.SetFloat("MoveZ", 0);
            transform.rotation = combat.lockedRotation;
            return;
        }

        animator.SetFloat("MoveZ", moveInput.magnitude);

        Transform cam = Camera.main.transform;

        Vector3 camForward = cam.forward;
        camForward.y = 0f;

        if (camForward.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(camForward);
        }

        Vector3 camRight = cam.right;
        camRight.y = 0f;

        Vector3 moveDir =
            camForward.normalized * moveInput.y +
            camRight.normalized * moveInput.x;

        if (moveDir.sqrMagnitude > 0.01f)
        {
            controller.Move(moveDir * moveSpeed * Time.deltaTime);
        }
    }
}
