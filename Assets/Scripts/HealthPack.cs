
using UnityEngine;
using Unity.Netcode;

public class HealthPack : NetworkBehaviour
{
    public int healAmount = 30;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;

        PlayerHealth health =
            other.GetComponent<PlayerHealth>();

        if (health == null) return;

        health.Heal(healAmount);

        NetworkObject.Despawn();
    }
}
