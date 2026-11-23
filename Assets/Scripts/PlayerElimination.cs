
using UnityEngine;
using Unity.Netcode;

public class PlayerElimination : NetworkBehaviour
{
    public NetworkVariable<bool> IsEliminated =
        new NetworkVariable<bool>(
            false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );

    public override void OnNetworkSpawn()
    {
        IsEliminated.OnValueChanged += OnEliminationChanged;
    }

    public override void OnNetworkDespawn()
    {
        IsEliminated.OnValueChanged -= OnEliminationChanged;
    }

    public void EliminatePlayer()
    {
        if (!IsServer || IsEliminated.Value)
            return;

        IsEliminated.Value = true;

        Debug.Log("PLAYER ELIMINATED!");
        EliminationMessageRpc(OwnerClientId);
    }

    private void OnEliminationChanged(bool oldValue, bool newValue)
    {
        if (newValue)
        {
            Debug.Log("A PLAYER HAS BEEN ELIMINATED!");
        }
    }

    [Rpc(SendTo.Everyone)]
    private void EliminationMessageRpc(ulong playerId)
    {
        Debug.Log("Player " + playerId + " has been eliminated!");
    }
}
