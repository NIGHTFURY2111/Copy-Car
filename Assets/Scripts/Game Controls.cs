using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControls : MonoBehaviour
{
    [SerializeField] Text gameSpeedText;
    CarAgent[] cAgent;


    private void Start()
    {
        cAgent = FindObjectsOfType<CarAgent>();
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            // Restart the game
            RestartGame();
        }
    }


    public void RestartGame()
    {
        // Restart the game
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        foreach (var agent in cAgent)
        {
            agent.EndEpisode();
        }
    }

    public void ChangeGameSpeed(float NewGameSpeed)
    {
        // Change the game speed
        Time.timeScale = NewGameSpeed;
        gameSpeedText.text = $"x {NewGameSpeed.ToString("F1")}";
    }
}
