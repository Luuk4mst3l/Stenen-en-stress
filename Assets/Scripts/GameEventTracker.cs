using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameEventTracker : MonoBehaviour
{
    [SerializeField]
    private float levelDuration = 0f;

    [SerializeField]
    private Image levelProgressBar;

    [SerializeField]
    private GameObject mainBuildingTimer;

    [SerializeField]
    private GameObject startPanel;

    [SerializeField]
    private GameObject pausePanel;

    [SerializeField]
    private GameObject gameOverPanel;

    InputAction pauseAction;


    private bool isLevelRunning = false;
    private float elapsedTime = 0f;

    private void Start()
    {   
        startPanel.SetActive(true);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        mainBuildingTimer.SetActive(false);
        isLevelRunning = false;
    }

    public void StartLevel()
    {
        mainBuildingTimer.SetActive(true);
        isLevelRunning = true;
        elapsedTime = 0f;
        startPanel.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            PauseGame();
        }
    }

    private void FixedUpdate()
    {
        if (isLevelRunning)
        {
            elapsedTime += Time.fixedDeltaTime;
            levelProgressBar.fillAmount = Mathf.Clamp01(elapsedTime / levelDuration);

            if (elapsedTime >= levelDuration)
            {
                Debug.Log("game over");
                EndLevel();
                isLevelRunning = false;
            }
        }
    }

    public void PauseGame()
    {
        if (isLevelRunning)
        {
            isLevelRunning = false;
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            isLevelRunning = true;
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    private void EndLevel()
    {   
        isLevelRunning = false;
        mainBuildingTimer.SetActive(false);
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        CheckValues();
        Debug.Log("Level Ended");
    }

    void CheckValues()
    {   
        Debug.Log("Checking values...");
        // Check voor relationship statuses en bouw requirements
    }
}
