using System.Collections;
using UnityEngine;

public class GameEventTracker : MonoBehaviour
{
    [SerializeField]
    private float levelDuration = 300f;

    public void StartLevel()
    {
        StartCoroutine("GameSequence");
    }

    IEnumerator GameSequence()
    {
        yield return new WaitForSeconds(levelDuration);
        Debug.Log("game over");
        EndLevel();
    }

    private void EndLevel()
    {
        //Logic
    }

    void CheckValues()
    {
        // Check voor relationship statuses en bouw requirements
    }
}
