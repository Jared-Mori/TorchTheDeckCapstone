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
}
