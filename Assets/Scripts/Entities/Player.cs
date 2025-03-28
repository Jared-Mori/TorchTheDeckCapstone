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
        entityName = "Player";
        entityType = EntityType.Player;
        facing = Direction.Up;
        viewDistance = 1;
        maxHealth = 10;
        health = maxHealth;
        maxEnergy = 3;
        energy = maxEnergy;
        artwork = levelManager.spriteManager.playerSprites[0];

        SetPosition(new Vector3Int(0, 0, 0));

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

    public void Equip(Equipment equipment)
    {
        switch (equipment.equipmentType)
        {
            case EquipmentType.Helmet:
                if (gear[0] != null) { gear[0].AddToDeck(deck, this); }
                gear[0] = equipment;
                break;
            case EquipmentType.Chestpiece:
                if (gear[1] != null) { gear[1].AddToDeck(deck, this); }
                gear[1] = equipment;
                break;
            case EquipmentType.Boots:
                if (gear[2] != null) { gear[2].AddToDeck(deck, this); }
                gear[2] = equipment;
                break;
            case EquipmentType.Shield:
                if (gear[3] != null) { gear[3].AddToDeck(deck, this); }
                gear[3] = equipment;
                break;
            case EquipmentType.Accessory:
                if (gear[4] != null) { gear[4].AddToDeck(deck, this); }
                gear[4] = equipment;
                break;
        }
    }
}
