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
        game_speed = initial_game_speed;
        enabled = true;
        spawner.gameObject.SetActive(true);
        game_over_text.gameObject.SetActive(false);
        retry_button.gameObject.SetActive(false);
    }
    public void GameOver()
    {
        game_speed = 0f;
        enabled = false;
        spawner.gameObject.SetActive(false);
        game_over_text.gameObject.SetActive(true);
        retry_button.gameObject.SetActive(true);
    }
    private void Update()
    {
        game_speed += game_speed_increase * Time.deltaTime;
    }
}
