
using UnityEngine;
using Unity.Netcode;

public class WeaponSystem : NetworkBehaviour
{
    public int damage = 25;
    public int headshotDamage = 100;
    public int magazineSize = 30;
    public float fireRate = 0.12f;
    public float range = 150f;

    public Camera aimCamera;

    private int currentAmmo;
    private float nextFireTime;
    private float nextServerFireTime;
    private bool isFiring;

    private Vector2 aimScreenPosition;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
            currentAmmo = magazineSize;

        if (IsOwner && aimCamera == null)
            aimCamera = Camera.main;

        aimScreenPosition = new Vector2(
            Screen.width / 2f,
            Screen.height / 2f
        );
    }

    private void Update()
    {
        if (!IsOwner || !isFiring)
            return;

        if (Time.time < nextFireTime)
            return;

        if (aimCamera == null)
            aimCamera = Camera.main;

        if (aimCamera == null)
            return;

        Ray ray = aimCamera.ScreenPointToRay(
            aimScreenPosition
        );

        ShootServerRpc(ray.direction);

        nextFireTime = Time.time + fireRate;
    }

    public void StartFiring()
    {
        if (!IsOwner) return;

        isFiring = true;
        nextFireTime = 0f;
    }

    public void StopFiring()
    {
        isFiring = false;
    }

    public void SetAimScreenPosition(Vector2 position)
    {
        if (!IsOwner) return;

        aimScreenPosition = position;
    }

    [Rpc(SendTo.Server)]
    private void ShootServerRpc(Vector3 direction)
    {
        if (currentAmmo <= 0)
            return;

        if (Time.time < nextServerFireTime)
            return;

        nextServerFireTime = Time.time + fireRate;
        currentAmmo--;

        Vector3 origin = transform.position +
                         Vector3.up * 1.5f;

        direction.Normalize();

        if (Physics.Raycast(
            origin,
            direction,
            out RaycastHit hit,
            range))
        {
            HitDetection target =
                hit.collider.GetComponentInParent<HitDetection>();

            if (target != null)
            {
                bool isHeadshot =
                    hit.collider.CompareTag("Head");

                target.bodyDamage = damage;
                target.headshotDamage = headshotDamage;

                target.ApplyHit(isHeadshot);

                Debug.Log(isHeadshot
                    ? "HEADSHOT!"
                    : "BODY HIT!");
            }
        }
    }

    [Rpc(SendTo.Server)]
    public void ReloadServerRpc()
    {
        currentAmmo = magazineSize;
    }
}
