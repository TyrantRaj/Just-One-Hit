using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public bool invulnerable;

    public void TakeDamage(int damage)
    {
        if (invulnerable) return;
        Debug.Log("Boss took damage: " + damage);
    }
}
