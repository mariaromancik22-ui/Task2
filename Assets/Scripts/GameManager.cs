using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float initial_game_speed = 5f;
    public float game_speed_increase = 0.1f;
    public float game_speed { get; private set; }

    private Player player;
    private Spawner spawner;

    public TextMeshProUGUI game_over_text;
    public Button retry_button;

    public TextMeshProUGUI score_text;
    public TextMeshProUGUI high_score_text;
    private float score;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            DestroyImmediate(gameObject);

    }
    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    private void Start()
    {
        player = FindAnyObjectByType<Player>();
        spawner = FindAnyObjectByType<Spawner>();
        NewGame();
        
    }
    public void NewGame()
    {
        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();
        foreach (var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }
        score = 0f;
        game_speed = initial_game_speed;
        enabled = true;
        spawner.gameObject.SetActive(true);
        game_over_text.gameObject.SetActive(false);
        retry_button.gameObject.SetActive(false);
        update_high_score();
    }
    public void GameOver()
    {
        game_speed = 0f;
        enabled = false;
        spawner.gameObject.SetActive(false);
        game_over_text.gameObject.SetActive(true);
        retry_button.gameObject.SetActive(true);
        update_high_score();
    }
    private void Update()
    {
        game_speed += game_speed_increase * Time.deltaTime;
        score += game_speed * Time.deltaTime;
        score_text.text = Mathf.FloorToInt(score).ToString("D5");
    }

    private void update_high_score()
    {
        float high_score = PlayerPrefs.GetFloat("high_score", 0);

        if (score > high_score)
        {
            high_score = score;
            PlayerPrefs.SetFloat("high_score", high_score);
            PlayerPrefs.Save();
        }
        high_score_text.text = Mathf.FloorToInt(high_score).ToString("D5");
    }
}
