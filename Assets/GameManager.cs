using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// holds game wide logic and state
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public InputActionReference cursorLock;
    public InputActionReference cursorFree;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        LockCursor();
    }

    // Update is called once per frame
    void Update()
    {
        if (cursorFree.action.WasPressedThisFrame()){
            UnlockCursor();
        }
        if (cursorLock.action.WasPressedThisFrame()){
            LockCursor();
        }
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
