using UnityEngine;
using UnityEngine.InputSystem;

// Heart that player picks up keybind E and give another life
public class HeartPickup : PlayerTrigger
{
    // Update is called once per frame
    void Update()
    {
        if (playerNearby && Keyboard.current.eKey.wasPressedThisFrame)
        {
            int level = GameManager.Instance.mazeLoader.currentLevel;

            // Only disappears if it was used (lives not full)
            if (GameManager.Instance.TryUseHeart(level))
            {
                SoundManager.Play(Sfx.Heart);
                Destroy(gameObject);
            }
        }
    }
}
