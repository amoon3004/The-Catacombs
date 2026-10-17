using UnityEngine;

// Base class for things the player interacts with tracking if the player is touching it
public abstract class PlayerTrigger : MonoBehaviour
{
    protected bool playerNearby;

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerNearby = true;
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerNearby = false;
    }
}
