using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class Player : Entity
{
    private float inputBuffer = 0.1f; // Duration between movements in seconds
    private float inputCooldownTimer = 0f; // Timer to track cooldown
    InputAction interactAction, moveAction;

    public override void SetDefaults()
    {
        Debug.Log("Setting default player values");
        entityType = EntityType.Player;
        viewDistance = 1;
        maxHealth = 10;
        maxEnergy = 3;
        energy = maxEnergy;
        sprite = levelManager.spriteManager.playerSprites[0];

        if (!isLoaded) 
        {
            facing = Direction.Up;
            SetPosition(new Vector3Int(0, 0, 0));
            health = maxHealth;
        } else
        {
            SetPosition(loadPosition);
        }

        // Keybindings
        moveAction = InputSystem.actions.FindAction("Move");
        interactAction = InputSystem.actions.FindAction("Interact");
    }

    // Update is called once per frame
    void Update()
    {
        // Update the cooldown timer
        if (inputCooldownTimer > 0)
        {
            inputCooldownTimer -= Time.deltaTime;
        }

        // Check if the cooldown has expired before allowing movement
        if (inputCooldownTimer <= 0)
        {
            if (moveAction.IsPressed())
            {
                Movement(moveAction.ReadValue<UnityEngine.Vector2>());
            }

            inputCooldownTimer = inputBuffer; // Reset the cooldown timer
        }

        if (interactAction.IsPressed())
        {
            Interact();
        }

    }

    public override void Interact()
    {
        Entity target = CheckView();
        if (target != null)
        {
            target.Interact();
        }
    }

    public void Movement(UnityEngine.Vector2 input)
    {
        switch (input)
        {
            case var _ when input == Vector2Int.up:
                facing = Direction.Up;
                break;
            case var _ when input == Vector2Int.down:
                facing = Direction.Down;
                break;
            case var _ when input == Vector2Int.left:
                facing = Direction.Left;
                break;
            case var _ when input == Vector2Int.right:
                facing = Direction.Right;
                break;
        }
       Move();
    }
}
