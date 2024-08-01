using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager current;
    public bool MenuOpenCloseInput { get; private set; }

    private PlayerInput _playerInput;
    private InputAction _menuOpenCloseAction;

    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }

        _playerInput = GetComponent<PlayerInput>();
        _menuOpenCloseAction = _playerInput.actions["MainMenuOpenClose"];
    }
    
    void Update()
    {
        MenuOpenCloseInput = _menuOpenCloseAction.WasPressedThisFrame();
    }
}
