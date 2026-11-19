# BattleMobile
My original 3D online multiplayer battle royale game project.

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
        {
            Health.Value = maximumHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        if (!IsServer || damage <= 0)
            return;

        Health.Value = Mathf.Max(
            0,
            Health.Value - damage
        );

        if (Health.Value <= 0)
        {
            Debug.Log("PLAYER ELIMINATED!");
        }
    }

    public void Heal(int amount)
    {
        if (!IsServer || amount <= 0)
            return;

        Health.Value = Mathf.Min(
            maximumHealth,
            Health.Value + amount
        );

        Debug.Log("PLAYER HEALED!");
    }
}

using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed = 5f;
    public float jumpHeight = 1.5f;
    public float gravity = -15f;

    private CharacterController controller;
    private MobileJoystick joystick;

    private Vector2 movementInput;
    private float verticalVelocity;
    private bool jumpRequested;

    private float nextSendTime;
    private const float SendInterval = 0.05f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            joystick = FindFirstObjectByType<MobileJoystick>();
        }
    }

    private void Update()
    {
        if (!IsSpawned)
            return;

        if (IsOwner)
        {
            ReadPlayerInput();

            if (Time.time >= nextSendTime)
            {
                SendMovementRpc(
                    movementInput,
                    jumpRequested
                );

                jumpRequested = false;
                nextSendTime = Time.time + SendInterval;
            }
        }

        if (IsServer)
        {
            ApplyMovement();
        }
    }

    private void ReadPlayerInput()
    {
        if (joystick != null)
        {
            movementInput = joystick.InputDirection;
        }
        else
        {
            movementInput = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            );
        }

        movementInput =
            Vector2.ClampMagnitude(movementInput, 1f);
    }

    public void Jump()
    {
        if (IsOwner)
        {
            jumpRequested = true;
        }
    }

    [Rpc(SendTo.Server)]
    private void SendMovementRpc(
        Vector2 input,
        bool jump,
        RpcParams rpcParams = default)
    {
        if (rpcParams.Receive.SenderClientId != OwnerClientId)
            return;

        movementInput =
            Vector2.ClampMagnitude(input, 1f);

        if (jump)
        {
            jumpRequested = true;
        }
    }

    private void ApplyMovement()
    {
        if (!controller.enabled)
            return;

        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        if (jumpRequested && controller.isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(
                jumpHeight * -2f * gravity
            );
        }

        jumpRequested = false;

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 direction = new Vector3(
            movementInput.x,
            0,
            movementInput.y
        );

        Vector3 velocity = direction * moveSpeed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);

        if (direction.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                10f * Time.deltaTime
            );
        }
    }
}
