using UnityEngine;
using UnityEngine.InputSystem;

// A power up so the player collects with E to see further for a time
public class TorchPickup : MonoBehaviour
{
    public float boostSeconds = 8f;

    TorchLight nearbyTorch; // set while player is touching item

    void OnTriggerEnter2D(Collider2D other)
    {
        // Player touches power up, remember their torch
        if (other.CompareTag("Player")) nearbyTorch = other.GetComponentInChildren<TorchLight>();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) nearbyTorch = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (nearbyTorch != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            nearbyTorch.Boost(boostSeconds);
            Destroy(gameObject);
        }
    }
}
