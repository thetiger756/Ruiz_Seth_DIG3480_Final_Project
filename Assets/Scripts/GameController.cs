using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject[] hazards;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public ParticleSystem nearStars;
    public ParticleSystem distantStars;

    public Text scoreText;
    public Text restartText;
    public Text hardModeText;
    public Text gameOverText;
    public Text youWinText;
    private int score;

    private bool gameOver;
    private bool youWin;
    private bool restart;
    private Coroutine co;

    private AudioSource audioSource;
    public AudioClip victoryMusic;
    public AudioClip defeatMusic;

    // Use this for initialization
    void Start () {
        gameOver = false;
        restart = false;
        restartText.text = "";
        hardModeText.text = "";
        gameOverText.text = "";
        youWinText.text = "";
        score = 0;
        UpdateScore();
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        co = StartCoroutine (SpawnWaves());
	}

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (gameOver)
        {
            restartText.text = "Press 'T' for Normal Mode";
            hardModeText.text = "Press 'Y' for Hard Mode";
            restart = true;
        }

        if (score >= 100)
        {
            YouWin();
            StopCoroutine(co);
            restartText.text = "Press 'T' for Normal Mode";
            hardModeText.text = "Press 'Y' for Hard Mode";
            restart = true;          
        }

        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                SceneManager.LoadScene(0);
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                SceneManager.LoadScene(1);
            }
        }

        if (youWin)
        {
            if (BGScroller.scrollSpeed > -5f)
            {
                BGScroller.scrollSpeed -= 0.005f;
            }
            if (BGScroller.scrollSpeed < -5f)
            {
                BGScroller.scrollSpeed = -5f;
            }
        }
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);

            if (gameOver || youWin)
            {
                //restartText.text = "Press 'T' to Restart";
                //restart = true;
                break;
            }
        }
    }

    public void AddScore (int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Points: " + score;
    }

    public void GameOver()
    {
        audioSource.Stop();
        audioSource.clip = defeatMusic;
        audioSource.Play();
        gameOverText.text = "Game Over!";
        gameOver = true;
    }

    public void YouWin()
    {
        var nearMain = nearStars.main;
        var distantMain = distantStars.main;

        nearMain.simulationSpeed += 25 * Time.deltaTime;
        nearMain.maxParticles = 1000;
        distantMain.simulationSpeed += 25 * Time.deltaTime;
        distantMain.maxParticles = 1000;

        if (youWin == false)
        {
            audioSource.Stop();
            audioSource.clip = victoryMusic;
            audioSource.Play();
            youWinText.text = "You Win! ----- Created by Seth Ruiz";
            youWin = true;
        }
    }
}
