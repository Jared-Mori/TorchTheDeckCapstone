using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public Vector2 movementInput;
    InputAction interactAction;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactAction = InputSystem.actions.FindAction("Interact");
    }

    void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
        //Debug.Log("Movement Input: " + movementInput);
    }
}
