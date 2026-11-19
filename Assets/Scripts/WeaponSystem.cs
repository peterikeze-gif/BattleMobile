using UnityEngine;
using Unity.Netcode;

public class WeaponSystem : NetworkBehaviour
{
    public int damage = 25;
    public int magazineSize = 30;
    public float fireRate = 0.2f;
    public float range = 100f;

    private int currentAmmo;
    private float nextFireTime;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
            currentAmmo = magazineSize;
    }

    public void Shoot()
    {
        if (!IsOwner) return;
        if (Time.time < nextFireTime) return;

        nextFireTime = Time.time + fireRate;
        ShootServerRpc();
    }

    [Rpc(SendTo.Server)]
    private void ShootServerRpc()
    {
        if (currentAmmo <= 0) return;

        currentAmmo--;

        // Shooting and hit detection will be connected
        // to the player's weapon camera in the next step.
    }

    [Rpc(SendTo.Server)]
    public void ReloadServerRpc()
    {
        currentAmmo = magazineSize;
    }
}