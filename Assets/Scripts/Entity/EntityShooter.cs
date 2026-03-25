using UnityEngine;

public class EntityShooter : MonoBehaviour
{
    private int _arrows;
    [SerializeField] Projectile projectilePrefab;

    public void Charge(int arrowsCount)
    {
        _arrows += arrowsCount;
    }

    public void TryShoot()
    {
        if (_arrows <= 0)
            return;

        Projectile p = Instantiate(projectilePrefab, transform.position + transform.up * 3 + transform.forward, transform.rotation);
        p.Shoot(transform.forward);

        _arrows--;
    }
}
