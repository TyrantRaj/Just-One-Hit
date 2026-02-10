using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public int damage = 1;
    public string targetTag = "Avoid";

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(targetTag)) return;

        Debug.Log("Hit " + other.name + "Damage" + damage.ToString());
    }
}
