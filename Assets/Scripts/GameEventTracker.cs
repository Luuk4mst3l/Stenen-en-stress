using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    [SerializeField]
    private NotifyStakeholders stakeholderNotifier;

    [SerializeField]
    private List<RelationshipTrackerUI> relationshipUIs;

    [SerializeField]
    private PlayerWallet playerWallet;

    [SerializeField]
    private DialogueRunner dialogueRunner;

    private LevelDialogueData activeDialogueData;

    private bool isLevelRunning = false;
    private float elapsedTime = 0f;

    private void Start()
    {
        startPanel.SetActive(true);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        mainBuildingTimer.SetActive(false);
        isLevelRunning = false;
        Time.timeScale = 0f;
    }

    public void StartLevel()
    {
        mainBuildingTimer.SetActive(true);
        isLevelRunning = true;
        elapsedTime = 0f;
        startPanel.SetActive(false);
        Time.timeScale = 1f;
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

    public void SetActiveDialogue(LevelDialogueData data)
    {
        activeDialogueData = data;
    }

    public void NotifyStakeholderForDialogue(int stakeholderIndex)
    {
        if (stakeholderNotifier != null)
        {
            stakeholderNotifier.NotifyStakeholder(stakeholderIndex);
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
        TMP_Text GameOverText = gameOverPanel
            .transform.Find("GameOverText")
            .GetComponent<TMP_Text>();
        GameObject restartButton = gameOverPanel.transform.Find("RestartLevelButton").gameObject;
        GameObject nextLevelButton = gameOverPanel.transform.Find("NextLevelButton").gameObject;

        isLevelRunning = false;
        mainBuildingTimer.SetActive(false);
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        if (CheckValues())
        {
            GameOverText.text = "Level Complete!";
            nextLevelButton.SetActive(true);
        }
        else
        {
            GameOverText.text = "Level Failed!";
            restartButton.SetActive(true);
        }
    }

    private bool CheckValues()
    {
        int relationships = 0;
        int goodRelationships = 0;
        foreach (var relationshipUI in relationshipUIs)
        {
            relationships++;
            if (relationshipUI.GetCurrentValue() >= 0)
            {
                goodRelationships++;
            }
        }
        if (dialogueRunner.checkFailedDialogue())
        {
            Debug.Log("Level failed due to missing dialogues!");
            return false;
        }
        if (playerWallet.Money >= 0 && goodRelationships >= relationships)
        {
            Debug.Log("Level Successful!");
            return true;
        }
        else
        {
            Debug.Log("Level Failed! Keep your relationships and money positive.");
            return false;
        }
    }
}
