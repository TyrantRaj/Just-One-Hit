using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour
{
    public Transform player;
    public float attackRange = 1.8f;
    public float moveSpeed = 3f;
    public float attackCooldown = 1.2f;
    public float dodgeDistance = 3f;
    public float dodgeCooldown = 1f;

    bool canAttack = true;
    bool canDodge = true;

    Animator animator;
    CharacterController controller;

    void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void OnEnable()
    {
        Hitbox.OnAnyHitboxEnabled += OnPlayerAttack;
    }

    void OnDisable()
    {
        Hitbox.OnAnyHitboxEnabled -= OnPlayerAttack;
    }

    void Update()
    {
        if (player == null) return;

        if (animator.GetBool("IsDodging")) return;

        Vector3 dir = player.position - transform.position;
        dir.y = 0;
        float dist = dir.magnitude;

        if (dist > attackRange)
        {
            controller.Move(dir.normalized * moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(dir);
            animator.SetFloat("MoveZ", 1);
        }
        else
        {
            animator.SetFloat("MoveZ", 0);

            if (canAttack && !animator.GetBool("IsAttacking"))
                StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        canAttack = false;
        animator.SetBool("IsAttacking", true);
        animator.SetTrigger("Attack_Light");
        yield return new WaitForSeconds(attackCooldown);
        animator.SetBool("IsAttacking", false);
        canAttack = true;
    }

    void OnPlayerAttack(Hitbox hitbox)
    {
        if (!canDodge) return;
        if (hitbox.owner == gameObject) return;
        if (!hitbox.owner.CompareTag("Player")) return;

        StartCoroutine(Dodge());
    }


    IEnumerator Dodge()
    {
        canDodge = false;

        animator.ResetTrigger("Attack_Light");
        animator.SetBool("IsAttacking", false);

        animator.SetBool("IsDodging", true);
        animator.SetTrigger("Dodge_Back");

        Vector3 dir = transform.position - player.position;
        dir.y = 0f;
        dir.Normalize();

        float timer = 0f;
        float duration = 0.25f;
        float speed = dodgeDistance / duration;

        while (timer < duration)
        {
            controller.Move(dir * speed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        animator.SetBool("IsDodging", false);

        yield return new WaitForSeconds(dodgeCooldown);
        canDodge = true;
    }

}
