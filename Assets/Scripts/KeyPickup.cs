using UnityEngine;
using UnityEngine.InputSystem;

// Key that the player picks up with keybind E
public class KeyPickup : PlayerTrigger
{
    // Update is called once per frame
    void Update()
    {
        if (playerNearby && Keyboard.current.eKey.wasPressedThisFrame)
        {
            GameManager.Instance.AddKey();
            SoundManager.Play(Sfx.Key);
            Destroy(gameObject);
        }
    }
}
