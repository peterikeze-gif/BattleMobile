
using UnityEngine;
using Unity.Netcode;

public class HealthPack : NetworkBehaviour
{
    public int healAmount = 30;

    private bool collected;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer || collected)
            return;

        PlayerHealth health =
            other.GetComponentInParent<PlayerHealth>();

        if (health == null)
            return;

        if (health.Health.Value <= 0)
            return;

        if (health.Health.Value >= health.maximumHealth)
            return;

        collected = true;

        health.Heal(healAmount);

        Debug.Log("HEALTH PACK COLLECTED! +30 HP");

        NetworkObject.Despawn();
    }
}
