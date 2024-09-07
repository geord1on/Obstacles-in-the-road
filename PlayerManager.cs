using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static bool gameOver;
    public GameObject gameOverPanel;

    public static bool isGameStarted;
    public GameObject startingText;
    public Button startButton; // Αναφορά για το κουμπί
    public static int numberOfCoins;
    public Text coinsText;
    public GameObject victoryPanel; // Νέα αναφορά για το VictoryPanel

    public int coinsToWin = 10; // Αριθμός κερμάτων που χρειάζονται για νίκη
    private AudioManager audioManager; // Αναφορά στον AudioManager

    void Start()
    {
        gameOver = false;
        Time.timeScale = 1;
        isGameStarted = false;
        numberOfCoins = 0;

        audioManager = FindObjectOfType<AudioManager>(); // Βρίσκουμε το AudioManager στη σκηνή

        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }
        else
        {
            Debug.LogError("startButton δεν έχει ανατεθεί στο Inspector.");
        }

        if (coinsText == null)
        {
            Debug.LogError("coinsText δεν έχει ανατεθεί στο Inspector.");
        }
        else
        {
            UpdateCoinsText();
        }

        // Αρχικά κρύβουμε το VictoryPanel
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("VictoryPanel δεν έχει ανατεθεί στο Inspector.");
        }
    }

    void Update()
    {
        if (gameOver)
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);

            if (audioManager != null)
            {
                audioManager.StopAllSounds(); // Σταματάει όλους τους ήχους
            }
        }

        UpdateCoinsText();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }

        // Έλεγχος αν ο παίκτης έχει συλλέξει αρκετά κέρματα
        if (numberOfCoins >= coinsToWin)
        {
            WinGame();
        }
    }

    void StartGame()
    {
        isGameStarted = true;
        Destroy(startingText);

        if (startButton != null)
        {
            startButton.gameObject.SetActive(false);
        }
    }

    void UpdateCoinsText()
    {
        if (coinsText != null)
        {
            coinsText.text = "Ζελεδάκια: " + numberOfCoins;
        }
    }

    // Νέα μέθοδος για να ανιχνεύει τη νίκη
    public void WinGame()
    {
        isGameStarted = false;
        Time.timeScale = 0;
        victoryPanel.SetActive(true); // Εμφάνιση του VictoryPanel

        if (audioManager != null)
        {
            audioManager.StopAllSounds(); // Σταματάει όλους τους ήχους
            audioManager.PlaySound("Winner"); // Παίζει το νικητήριο κομμάτι
        }
    }
}
