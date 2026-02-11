using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public static System.Action<Hitbox> OnAnyHitboxEnabled;

    public GameObject owner;
    public int damage = 1;
    public string targetTag = "Enemy";

    Collider col;

    void Awake()
    {
        col = GetComponent<Collider>();
        col.enabled = false;
    }

    public void Activate()
    {
        col.enabled = true;
        OnAnyHitboxEnabled?.Invoke(this);
    }

    public void Deactivate()
    {
        col.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(targetTag)) return;

        if (other.TryGetComponent<BossAI>(out var boss))
        {
            float dist = Vector3.Distance(owner.transform.position, other.transform.position);
            if (dist > boss.attackRange) return;
        }

        if (other.TryGetComponent<BossHealth>(out var health))
        {
            health.TakeDamage(damage);
        }

        if (other.TryGetComponent<PlayerHealth>(out var playerHealth))
        {
            Debug.Log("taking damage");
            playerHealth.TakeDamage(damage);
            
            Deactivate();
        }

    }



}
