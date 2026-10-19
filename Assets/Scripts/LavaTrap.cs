using UnityEngine;

public class LavaTrap : MonoBehaviour
{
    // Lava trap that if the player touches it, they will lose a life and restart the level
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.Play(Sfx.Lava);
            GameManager.Instance.PlayerDied();
        }
    }
}
