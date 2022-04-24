using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public bool isGameActive = false;

    private int score;
    private int shield;
    private int lowShield = 30;
    private int highShield = 60;
    private const int MAX_SHIELD = 150;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI shieldText;
    [SerializeField] private Color[] colorShieldText;
    [SerializeField] private GameObject uIScreen;

    private bool isGamePaused = false;
    [SerializeField] private GameObject pauseScreen;

    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private GameObject gameOverScreen;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private LeaderboardManager lbManager;

    public float effectsVolume = 0.75f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isGameActive)
        {
            PauseResumeGame();
        }
    }

    public void StartGame()
    {
        score = 0;
        UpdateScore(0);
        shield = 100;
        UpdateShield(0);
        uIScreen.SetActive(true);
        playerController.ResetPlayer();
        isGameActive = true;
        // More here
    }

    public void PauseResumeGame()
    {
        if (isGamePaused)
        {
            pauseScreen.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            pauseScreen.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        isGamePaused = !isGamePaused;
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        // Here change player to explosion + destroy delayed

        isGameActive = false;

        playerController.Explosion();

        spawnManager.DespawnAll();

        uIScreen.SetActive(false);
        finalScoreText.SetText("SCORE: " + score);
        if (lbManager.IsNewRecord(score))
        {
            // Here show UI input record name if its record
            // The following is for testing only, remove after
            string newName = "user" + Random.Range(0, 100);
            lbManager.AddNewRecord(newName, score);
        }
        gameOverScreen.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Game quited!");
        Application.Quit();
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.SetText("SCORE: " + score);
    }

    public void UpdateShield(int shieldToAdd)
    {
        shield = Mathf.Min(shield + shieldToAdd, MAX_SHIELD);
        shieldText.SetText("SHIELD: " + shield + '%');
        if (shield > highShield) shieldText.color = colorShieldText[2];
        else if (shield <= lowShield) shieldText.color = colorShieldText[0];
        else shieldText.color = colorShieldText[1];
        if (shield < 0)
        {
            shieldText.SetText("SHIELD: --");
            GameOver();
        }
    }

    public void SetEffectsVolume(float newEffectsVolume)
    {
        effectsVolume = newEffectsVolume;
    }
}
