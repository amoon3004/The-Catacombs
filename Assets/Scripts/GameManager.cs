using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;

// Tracks lives, keys, and on screen text for the playthrough
public class GameManager : MonoBehaviour
{
    // Lets other scripts reach the manager with GameManager.Instance
    public static GameManager Instance { get; private set; }

    [Header("Rules")]
    public int maxLives = 3;
    public float messageSeconds = 2f;

    [Header("On-Screen text")]
    public TMP_Text livesText;
    public TMP_Text keysText;
    public TMP_Text messageText;

    [Header("Scene references")]
    public MazeLoader mazeLoader;

    public int Lives { get; private set; }
    public int KeysHeld { get; private set; }
    public int KeysNeeded { get; private set; }
    public int KeysMissing => KeysNeeded - KeysHeld;

    // Levels whose heart was already used. Cleared when game is over
    readonly HashSet<int> heartsUsed = new HashSet<int>();
    float messageTimeLeft;
    bool gameWon;

    void Awake()
    {
        Instance = this;
        Lives = maxLives;
        messageText.text = "";
    }

    void Update()
    {
        // Hide the pop-up message once it runs out
        if (messageTimeLeft > 0f)
        {
            messageTimeLeft -= Time.deltaTime;

            if (messageTimeLeft <= 0f) messageText.text = "";
        }
    }

    // Called by MazeLoader each time level loads or restarts
    public void StartLevel(int keysInLevel)
    {
        KeysHeld = 0;
        KeysNeeded = keysInLevel;
        UpdateDisplay();
    }

    public void AddKey()
    {
        KeysHeld++;
        UpdateDisplay();
        ShowMessage("Picked up a key!");
    }

    public bool HeartUsed(int level)
    {
        return heartsUsed.Contains(level);
    }

    // Returns true if heart was used, false if lives are already full
    public bool TryUseHeart(int level)
    {
        if (Lives >= maxLives)
        {
            ShowMessage("Lives are full!");
            return false;
        }

        Lives++;
        heartsUsed.Add(level);
        UpdateDisplay();
        ShowMessage("+1 Life!");
        return true;
    }

    // Called by Lava
    public void PlayerDied()
    {
        Lives--;
        if (Lives > 0)
        {
            ShowMessage("You died! Lives left: " + Lives);
            mazeLoader.Load(mazeLoader.currentLevel);  // restart level
        }
        else
        {
            GameOver();
        }
    }

    void GameOver()
    {
        RestartGame();
        ShowMessage("GAME OVER!!! Back to the bottom of the Catacombs!", 3f);
    }

    // Called by MazeLoader when the last level door is opened
    public void Victory()
    {
        if (gameWon) return;
        gameWon = true;
        ShowMessage("You escaped the Catacombs!!!", 5f);
        SetPlayerFrozen(true);
        Invoke(nameof(RestartAfterVictory), 5f);
    }

    void RestartAfterVictory()
    {
        gameWon = false;
        SetPlayerFrozen(false);
        RestartGame();
    }

    // Full reset
    void RestartGame()
    {
        Lives = maxLives;
        heartsUsed.Clear();
        mazeLoader.currentLevel = 0;
        mazeLoader.Load(0);
    }

    // Turns player movement off or back on
    void SetPlayerFrozen(bool frozen)
    {
        mazeLoader.player.GetComponent<PlayerMovement>().enabled = !frozen;
    }

    public void ShowMessage(string text, float seconds = -1f)
    {
        messageText.text = text;
        messageTimeLeft = seconds > 0f ? seconds : messageSeconds;
    }

    void UpdateDisplay()
    {
        livesText.text = "Lives: " + Lives;
        keysText.text = "Keys: " + KeysHeld + "/" + KeysNeeded;
    }
}
