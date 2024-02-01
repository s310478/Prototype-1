using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// !!-- FIXES BUG: click on blank space on UI, cannot interact with UI using keyboard/controller --!!
public class UISelectionManager : MonoBehaviour
{
    private EventSystem eventSystem;
    private GameObject lastSelected;

    void Awake()
    {
        eventSystem = GetComponent<EventSystem>();
    }

    void Update()
    {
        // Check if there's a currently selected UI button/element
        if (eventSystem.currentSelectedGameObject != null)
        {
            // Store the current selected UI element
            lastSelected = eventSystem.currentSelectedGameObject;
        }
        else if (lastSelected != null)
        {
            // If there's no current selection and the last selected element is not null
            if ((Gamepad.current != null && GamepadWasUsed()) || KeyboardWasUsed())
            {
                // If a gamepad is connected and was used, or if relevant keyboard keys were used
                // Reselect the last selected UI element
                eventSystem.SetSelectedGameObject(lastSelected);
            }
        }
    }

    private bool GamepadWasUsed()
    {
        // Check if any relevant gamepad controls were used (D-pad or left stick)
        // Returns true if the D-pad or left stick is being moved
        return Gamepad.current.dpad.ReadValue() != Vector2.zero ||
            Gamepad.current.leftStick.ReadValue() != Vector2.zero;
    }

    private bool KeyboardWasUsed()
    {
        // Check if relevant keyboard keys were used (e.g., arrow keys, Enter)
        // You can add more keys if needed
        var keyboard = Keyboard.current;
        return keyboard != null &&
            (keyboard.wKey.wasPressedThisFrame || keyboard.sKey.wasPressedThisFrame || keyboard.enterKey.wasPressedThisFrame);
    }
}
