
using UnityEngine;
using Unity.Netcode;

public class HitDetection : NetworkBehaviour
{
    public int bodyDamage = 25;
    public int headshotDamage = 100;

    public void ApplyHit(bool isHeadshot)
    {
        if (!IsServer) return;

        int damage = isHeadshot
            ? headshotDamage
            : bodyDamage;

        PlayerHealth health =
            GetComponent<PlayerHealth>();

        if (health != null)
        {
            health.TakeDamage(damage);
        }
    }
}
