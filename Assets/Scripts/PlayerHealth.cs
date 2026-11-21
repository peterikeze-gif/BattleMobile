using UnityEngine;
using Unity.Netcode;

public class PlayerHealth : NetworkBehaviour
{
    public int maximumHealth = 100;

    public NetworkVariable<int> Health =
        new NetworkVariable<int>(
            100,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );

    public override void OnNetworkSpawn()
    {
        if (IsServer)
            Health.Value = maximumHealth;
    }

    public void TakeDamage(int damage)
    {
        if (!IsServer || damage <= 0) return;

        Health.Value = Mathf.Max(0, Health.Value - damage);
    }

    public void Heal(int amount)
    {
        if (!IsServer || amount <= 0) return;

        Health.Value = Mathf.Min(
            maximumHealth,
            Health.Value + amount
        );
    }
}