
using UnityEngine;
using Unity.Netcode;

public class HitDetection : NetworkBehaviour
{
    public int bodyDamage = 25;
    public int headshotDamage = 100;

    public void ApplyHit(bool isHeadshot)
    {
        if (!IsServer)
            return;

        int damage = isHeadshot
            ? headshotDamage
            : bodyDamage;

        PlayerHealth health =
            GetComponentInParent<PlayerHealth>();

        if (health == null)
            return;

        if (health.Health.Value <= 0)
            return;

        health.TakeDamage(damage);

        if (isHeadshot)
        {
            Debug.Log("HEADSHOT! 100 DAMAGE!");
        }
        else
        {
            Debug.Log("BODY HIT! 25 DAMAGE!");
        }

        if (health.Health.Value <= 0)
        {
            PlayerElimination elimination =
                GetComponentInParent<PlayerElimination>();

            if (elimination != null)
                elimination.EliminatePlayer();

            Debug.Log("PLAYER ELIMINATED!");
        }
    }
}
