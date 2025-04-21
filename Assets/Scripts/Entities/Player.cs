using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class Player : Entity
{
    private float inputBuffer = 0.1f; // Duration between movements in seconds
    private float inputCooldownTimer = 0f; // Timer to track cooldown
    InputAction interactAction, moveAction, menuAction;

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
        } 
        else
        {
            SetPosition(loadPosition);
        }

        // Keybindings
        moveAction = InputSystem.actions.FindAction("Move");
        interactAction = InputSystem.actions.FindAction("Interact");
        menuAction = InputSystem.actions.FindAction("Menu");

        // Subscribe to input events
        menuAction.performed += OnMenuAction;
        interactAction.performed += OnInteractAction;
    }

    private void OnDestroy()
    {
        // Unsubscribe from input events to avoid memory leaks
        menuAction.performed -= OnMenuAction;
        interactAction.performed -= OnInteractAction;
    }

    private void OnMenuAction(InputAction.CallbackContext context)
    {
        if (Time.timeScale != 0f)
        {
            Menues.PauseGame();
        }
        else
        {
            Menues.ResumeGame();
        }
    }

    private void OnInteractAction(InputAction.CallbackContext context)
    {
        Debug.Log("Interact action performed");
        Interact();
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
        if (inputCooldownTimer <= 0 && moveAction.IsPressed())
        {
            Movement(moveAction.ReadValue<UnityEngine.Vector2>());
            inputCooldownTimer = inputBuffer; // Reset the cooldown timer
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
