
using UnityEngine;
using Unity.Netcode;

public class BulletSystem : NetworkBehaviour
{
    public float speed = 80f;
    public float lifetime = 3f;

    private Vector3 direction;
    private ulong shooterId;

    public void Initialize(Vector3 shotDirection,
                           ulong ownerId)
    {
        if (!IsServer) return;

        direction = shotDirection.normalized;
        shooterId = ownerId;
    }

    private void Update()
    {
        if (!IsServer) return;

        transform.position +=
            direction * speed * Time.deltaTime;

        lifetime -= Time.deltaTime;

        if (lifetime <= 0)
            NetworkObject.Despawn();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;

        if (other.TryGetComponent<HitDetection>(
            out HitDetection hit))
        {
            if (other.GetComponent<NetworkObject>()
                ?.OwnerClientId != shooterId)
            {
                hit.ApplyHit(false);
            }

            NetworkObject.Despawn();
        }
    }
}
