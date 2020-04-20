using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnd : MonoBehaviour
{
    [SerializeField]
    Health[] spawners = null;

    [SerializeField]
    Health[] humans = null;

    [SerializeField]
    GameObject winText = null;

    [SerializeField]
    GameObject loseText = null;

    [SerializeField]
    GameObject restartText = null;

    [SerializeField]
    GameStart gameStart = null;

    bool gameOver = false;

    void Update()
    {
        if (!gameStart.gameStarted)
            return;

        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Return))
                SceneManager.LoadScene("Game");

            return;
        }

        bool allSpawnersDead = true;
        foreach (var spawner in spawners)
        {
            if (spawner.health > 0)
            {
                allSpawnersDead = false;
                break;
            }
        }

        if (allSpawnersDead)
        {
            winText.SetActive(true);
            restartText.SetActive(true);
            gameOver = true;
        }

        bool allHumansDead = true;
        foreach (var human in humans)
        {
            if (human.tag == "Human" && human.health > 0)
            {
                allHumansDead = false;
                break;
            }
        }

        if (allHumansDead)
        {
            loseText.SetActive(true);
            restartText.SetActive(true);
            gameOver = true;
        }
    }
}
