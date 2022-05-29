using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private GameObject saveButtonGO;
    [SerializeField] private GameObject gameOverScreen;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private LeaderboardManager lbManager;

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
            nameInputField.text = "";
            nameInputField.gameObject.SetActive(true);
            saveButtonGO.SetActive(true);
        }
        else
        {
            nameInputField.gameObject.SetActive(false);
            saveButtonGO.SetActive(false);
        }
        gameOverScreen.SetActive(true);
    }

    // TODO: check if the name is valid (0 < length <= 8) if not show a message inside InputField and don't allow to be added to record
    public void SaveNewRecord()
    {
        lbManager.AddNewRecord(nameInputField.text, score);
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
}
