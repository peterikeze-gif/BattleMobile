
using UnityEngine;
using Unity.Netcode;

public class PlayerScore : NetworkBehaviour
{
    public NetworkVariable<int> Score =
        new NetworkVariable<int>(
            0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );

    public void AddKillPoint()
    {
        if (!IsServer) return;

        Score.Value++;
    }
}
