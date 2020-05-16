using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    [SerializeField]
    private Health[] playerHealths;
    [SerializeField]
    private string gameOverMessage = "GAME  OVER\nPRESS  SPACE  TO  RESTART";
    [SerializeField]
    private Text gameOverText;

    private bool gameOver = false;
    private void Update()
    {
        if(gameOver && Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        foreach(Health health in playerHealths)
        {
            if(health.GetHealth() > 0)
            {
                // Something is alive still, not game over
                return;
            }
        }

        gameOverText.text = gameOverMessage;
        gameOver = true;
        Time.timeScale = 0f;
    }
}
