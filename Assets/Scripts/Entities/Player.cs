using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class Player : Entity
{
    private float inputBuffer = 0.1f; // Duration between movements in seconds
    private float inputCooldownTimer = 0f; // Timer to track cooldown
    InputAction interactAction, moveAction, menuAction;
    private Rigidbody2D rb;
    int moveSpeed = 5;

    public override void SetDefaults()
    {
        Debug.Log("Setting default player values");
        entityType = EntityType.Player;
        viewDistance = 1;
        maxHealth = 10;
        health = maxHealth;
        maxEnergy = 3;
        energy = maxEnergy;

        rb = GetComponent<Rigidbody2D>();

        // Keybindings
        moveAction = InputSystem.actions.FindAction("Move");
        moveAction.canceled += OnMoveCanceled; // Subscribe to input release
        interactAction = InputSystem.actions.FindAction("Interact");
        menuAction = InputSystem.actions.FindAction("Menu");

        // Subscribe to input events
        menuAction.performed += OnMenuAction;
        interactAction.performed += OnInteractAction; // Subscribe to Interact key press
    }

    private void OnDestroy()
    {
        // Unsubscribe from input events to avoid memory leaks
        menuAction.performed -= OnMenuAction;
        moveAction.canceled -= OnMoveCanceled;
        interactAction.performed -= OnInteractAction; // Unsubscribe from Interact key press
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
        OnInteract(null); // Call the existing OnInteract method
    }

    private void OnInteract(InputAction input)
    {
        Entity target = CheckView();
        if (target != null)
        {
            target.Interact();
        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        // Stop movement when input is released
        rb.linearVelocity = Vector2.zero;
        UpdateAnimator(Vector2.zero); // Update animator to idle
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

    public void Movement(Vector2 input)
    {
        Vector2 normalizedInput = input.normalized;

        if (normalizedInput != Vector2.zero)
        {
            // Update the facing direction
            if (normalizedInput.x > 0) facing = Direction.Right;
            else if (normalizedInput.x < 0) facing = Direction.Left;
            if (normalizedInput.y > 0) facing = Direction.Up;
            else if (normalizedInput.y < 0) facing = Direction.Down;

            // Apply movement using Rigidbody2D
            rb.linearVelocity = normalizedInput * moveSpeed;

            // Update animator and sprite flipping
            UpdateAnimator(normalizedInput);
        }
        else
        {
            // Stop movement when no input is detected
            rb.linearVelocity = Vector2.zero;
            UpdateAnimator(Vector2.zero); // Update animator to idle
        }
    }

    private void UpdateAnimator(Vector2 movement)
    {
        // Update the Speed parameter in the Animator
        animator.SetFloat("Speed", movement.magnitude);

        // Flip the sprite based on the facing direction
        if (movement.x != 0)
        {
            spriteRenderer.flipX = movement.x < 0; // Flip if moving left
        }
    }

    public override Entity CheckView()
    {
        // Get the direction the entity is facing
        Vector2 direction = Directions[facing];

        // Offset the boxcast origin slightly in the direction of the ray
        Vector2 boxSize = new Vector2(0.5f, 0.5f); // Adjust to match the player's size
        Vector2 boxOrigin = (Vector2)transform.position + direction * 0.5f; // Offset by half the box size

        // Define a layer mask to exclude the Player and Walls layers
        int layerMask = ~LayerMask.GetMask("Player", "Wall");

        // Perform the boxcast
        RaycastHit2D hit = Physics2D.BoxCast(boxOrigin, boxSize, 0f, direction, viewDistance, layerMask);

        // Debug the boxcast
        Vector2 boxCorner1 = boxOrigin - boxSize / 2;
        Vector2 boxCorner2 = boxOrigin + boxSize / 2;
        Debug.DrawLine(boxCorner1, new Vector2(boxCorner1.x, boxCorner2.y), Color.green, 0.1f); // Left edge
        Debug.DrawLine(boxCorner1, new Vector2(boxCorner2.x, boxCorner1.y), Color.green, 0.1f); // Bottom edge
        Debug.DrawLine(boxCorner2, new Vector2(boxCorner1.x, boxCorner2.y), Color.green, 0.1f); // Top edge
        Debug.DrawLine(boxCorner2, new Vector2(boxCorner2.x, boxCorner1.y), Color.green, 0.1f); // Right edge

        // Check if the boxcast hit something
        if (hit.collider != null)
        {
            Debug.Log($"BoxCast hit: {hit.collider.gameObject.name} on layer {LayerMask.LayerToName(hit.collider.gameObject.layer)}");

            Entity hitEntity = hit.collider.GetComponent<Entity>();
            if (hitEntity != null)
            {
                Debug.Log($"{hitEntity.entityType} detected at {hit.point}");
                return hitEntity; // Return the detected entity
            }
            else
            {
                Debug.Log("Hit object does not have an Entity component");
            }
        }

        Debug.Log("No entity detected in view");
        return null; // No entity detected
    }
}
