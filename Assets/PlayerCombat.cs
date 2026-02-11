using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    Animator animator;

    [SerializeField] Hitbox righthand;
    [SerializeField] Hitbox lefthand;
    [SerializeField] Hitbox leg;

    public Quaternion lockedRotation;

    void Awake()
    {
        animator = GetComponent<Animator>();

        righthand.owner = gameObject;
        lefthand.owner = gameObject;
        leg.owner = gameObject;
    }


    public void OnLightAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        StartAttack("Attack_Light");
    }

    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        StartAttack("Attack_Heavy");
    }

    public void OnKick(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        StartAttack("Attack_Kick");
    }

    void StartAttack(string trigger)
    {
        if (animator.GetBool("IsDodging")) return;

        lockedRotation = transform.rotation;
        animator.ResetTrigger("Attack_Light");
        animator.ResetTrigger("Attack_Heavy");
        animator.ResetTrigger("Attack_Kick");

        animator.SetBool("IsAttacking", true);
        animator.SetTrigger(trigger);
    }

    public void EndAttack()
    {
        animator.SetBool("IsAttacking", false);
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        animator.ResetTrigger("Attack_Light");
        animator.ResetTrigger("Attack_Heavy");
        animator.ResetTrigger("Attack_Kick");

        animator.SetBool("IsAttacking", false);

        lockedRotation = transform.rotation;
        animator.SetBool("IsDodging", true);
        animator.SetTrigger("Dodge_Back");
    }

    public void EndDodge()
    {
        animator.SetBool("IsDodging", false);
    }

    public void EnableRightHand() => righthand.Activate();
    public void DisableRightHand() => righthand.Deactivate();
    public void EnableLeftHand() => lefthand.Activate();
    public void DisableLeftHand() => lefthand.Deactivate();
    public void EnableLeg() => leg.Activate();
    public void DisableLeg() => leg.Deactivate();
}
