using UnityEngine;
using UnityEngine.InputSystem;

// The exit, pressing F opens it if player has every key for that level
public class ExitDoor : PlayerTrigger
{
    // Update is called once per frame
    void Update()
    {
        if (!playerNearby || !Keyboard.current.fKey.wasPressedThisFrame) return;

        int missing = GameManager.Instance.KeysMissing;

        if (missing <= 0)
        {
            SoundManager.Play(Sfx.DoorOpen);
            GameManager.Instance.ShowMessage("The door has been opened...");
            GameManager.Instance.mazeLoader.NextLevel();
        }
        else if (missing == 1)
        {
            SoundManager.Play(Sfx.DoorClose);
            GameManager.Instance.ShowMessage("Requires 1 more key to open...");
        }
        else
        {
            SoundManager.Play(Sfx.DoorClose);
            GameManager.Instance.ShowMessage("Requires " + missing + " more keys to open...");
        }
    }
}
