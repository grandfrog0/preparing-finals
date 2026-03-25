using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Damage;
    public float Strength;

    public void Shoot(Vector3 forward)
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        transform.forward = forward;
        rigidbody.linearVelocity = forward * Strength;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        if (other.TryGetComponent(out Entity entity))
        {
            if (!entity.IsDead)
            {
                entity.TakeDamage(Damage);
            }
        }
        Destroy(gameObject);
    }
}
