
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

    public void EliminatePlayer()
    {
        if (!IsServer || IsEliminated.Value)
            return;

        IsEliminated.Value = true;

        Debug.Log("Player eliminated!");
    }
}
