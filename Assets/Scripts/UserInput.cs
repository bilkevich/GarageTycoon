using UnityEngine;

public class UserInput : MonoBehaviour
{
    public InputSystem_Actions inputSystemActions;
    private void Awake()
    {
        inputSystemActions = new InputSystem_Actions();
        
        inputSystemActions.UI.Enable();
    }
}