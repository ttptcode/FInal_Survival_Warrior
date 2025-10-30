// using UnityEngine;

// public class InputActionsUpdater : MonoBehaviour
// {
//     [Header("Input Actions Setup")]
//     [TextArea(5, 10)]
//     public string instructions = @"
// Để thêm input actions cho gun system:

// 1. Mở InputSystem_Actions.inputactions
// 2. Trong Player map, thêm các actions sau:

// Fire (Button):
// - Mouse Left Button
// - Gamepad Right Trigger

// Reload (Button):
// - Keyboard R
// - Gamepad Y Button

// Aim (Vector2):
// - Mouse Position (cho mouse aiming)
// - Gamepad Right Stick (cho controller aiming)

// 3. Assign các InputActionReference trong PlayerGunController:
// - fireAction = Fire action
// - reloadAction = Reload action  
// - aimAction = Aim action
// ";

//     [ContextMenu("Show Current Input Actions")]
//     public void ShowCurrentInputActions()
//     {
//         Debug.Log("Current Input Actions:");
//         Debug.Log("- Move: WASD/Arrow Keys/Gamepad Left Stick");
//         Debug.Log("- Look: Mouse Delta/Gamepad Right Stick");
//         Debug.Log("- Attack: Mouse Left Button/Gamepad West Button");
//         Debug.Log("- Jump: Space/Gamepad South Button");
//         Debug.Log("- Sprint: Left Shift/Gamepad Left Stick Press");
//         Debug.Log("- Interact: E/Gamepad North Button");
//         Debug.Log("- Crouch: C/Gamepad East Button");
//         Debug.Log("- Previous: 1/Gamepad Dpad Left");
//         Debug.Log("- Next: 2/Gamepad Dpad Right");
        
//         Debug.Log("\nCần thêm cho Gun System:");
//         Debug.Log("- Fire: Mouse Left Button (đã có sẵn)");
//         Debug.Log("- Reload: R key");
//         Debug.Log("- Aim: Mouse Position hoặc Gamepad Right Stick");
//     }
// }
