using JUTPS;
using JUTPS.JUInputSystem;
using UnityEngine;
using Input = UnityEngine.Windows.Input;

public class BaseCanvas : MonoBehaviour
{
    void Start()
    {
        Unlock();
    }

    void Update()
    {
        
    }

    void Unlock()
    {
        Cursor.lockState = CursorLockMode.None; // Unlocks the cursor
        Cursor.visible = true; // Makes the cursor visible
    }

    void Lock()
    {
        Cursor.lockState = CursorLockMode.Locked; // Locks the cursor to the center
        Cursor.visible = false; // Hides the cursor
    }
}
