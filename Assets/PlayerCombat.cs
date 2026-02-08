using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    Animator animator;
    PlayerInput playerInput;

    public Quaternion lockedRotation;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }

    public void OnLightAttack(InputAction.CallbackContext context)
    {
        if (!context.performed || animator.GetBool("IsAttacking") || animator.GetBool("IsDodging")) return;
        StartAttack("Attack_Light");
    }

    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        if (!context.performed || animator.GetBool("IsAttacking") || animator.GetBool("IsDodging")) return;
        StartAttack("Attack_Heavy");
    }

    public void OnKick(InputAction.CallbackContext context)
    {
        if (!context.performed || animator.GetBool("IsAttacking") || animator.GetBool("IsDodging")) return;
        StartAttack("Attack_Kick");
    }

    void StartAttack(string trigger)
    {
        lockedRotation = transform.rotation;
        animator.SetBool("IsAttacking", true);
        animator.SetTrigger(trigger);
        playerInput.enabled = false;
    }

    public void EndAttack()
    {
        animator.SetBool("IsAttacking", false);
        playerInput.enabled = true;
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (!context.performed || animator.GetBool("IsAttacking") || animator.GetBool("IsDodging")) return;
        StartDodge();
    }

    void StartDodge()
    {
        lockedRotation = transform.rotation;
        animator.SetBool("IsDodging", true);
        animator.SetTrigger("Dodge_Back");
        playerInput.enabled = false;
    }

    public void EndDodge()
    {
        animator.SetBool("IsDodging", false);
        playerInput.enabled = true;
    }
}
