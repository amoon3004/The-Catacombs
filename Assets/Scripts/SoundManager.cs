using UnityEngine;

// Every sound affect in the game. Other scripts call 
public enum Sfx { Key, Heart, Torch, DoorOpen, DoorClose, Lava, GameOver, Victory }

// Plays all of the game's sound affects from one place
public class SoundManager : MonoBehaviour
{
    static SoundManager instance;

    [Header("Audio sources")]
    public AudioSource effectsSource;
    public AudioSource footstepSource;

    [Header("Clips")]
    public AudioClip keyPickup;
    public AudioClip heartPickup;
    public AudioClip torchPickup;
    public AudioClip doorOpen;
    public AudioClip doorClose;
    public AudioClip lavaDeath;
    public AudioClip gameOver;
    public AudioClip victory;
    public AudioClip[] footsteps;

    void Awake()
    {
        instance = this;
    }

    // Play one sound affect
    public static void Play(Sfx sfx)
    {
        if (instance == null) return;
        AudioClip clip = instance.ClipFor(sfx);
        if (clip != null) instance.effectsSource.PlayOneShot(clip);
    }

    // Play a random footstep at a slightly random pitch
    public static void PlayFootstep()
    {
        if (instance == null || instance.footsteps.Length == 0) return;
        instance.footstepSource.pitch = Random.Range(0.9f, 1.1f);
        int i = Random.Range(0, instance.footsteps.Length);
        instance.footstepSource.PlayOneShot(instance.footsteps[i]);
    }

    // Matches each Sfx name to the clip assigned in the inspector
    AudioClip ClipFor(Sfx sfx)
    {
        switch (sfx)
        {
            case Sfx.Key: return keyPickup;
            case Sfx.Heart: return heartPickup;
            case Sfx.Torch: return torchPickup;
            case Sfx.DoorOpen: return doorOpen;
            case Sfx.DoorClose: return doorClose;
            case Sfx.Lava: return lavaDeath;
            case Sfx.GameOver: return gameOver;
            case Sfx.Victory: return victory;
        }
        return null;
    }
}
