using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float initial_game_speed = 5f;
    public float initial_game_speed_increase = 0.1f; //переделали, чтобы при проигрыше пичт снижался, земля и припятсвие не двигались
    //И ЧТОБЫ МОЖНО БЫЛО МЕНЯТЬ УСКОРЕНИЕ С ИНСПЕКТОРА В САМОМ ЮНИТИ
    public float game_speed { get; private set; }
    public float game_speed_increase { get; private set; }

    private Player player;
    private Spawner spawner;

    public TextMeshProUGUI game_over_text;
    public Button retry_button;

    public TextMeshProUGUI score_text;
    public TextMeshProUGUI high_score_text;
    private float score;

    public Toggle colorToggle;

    public AudioSource source;
    public float pitch_controller = 0.8f;
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
        player.dead = false;
        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();
        foreach (var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }
        score = 0f;
        game_speed = initial_game_speed;
        game_speed_increase = initial_game_speed_increase;
        enabled = true;
        spawner.gameObject.SetActive(true);
        game_over_text.gameObject.SetActive(false);
        retry_button.gameObject.SetActive(false);
        update_high_score();
        source.pitch = pitch_controller;
        pitch_controller = 0.8f;
    }
    public void GameOver()
    {
        //Time.timeScale = 0f;
        game_speed = 0;
        game_speed_increase = 0;
        //enabled = false; //не будет работать питч контроллер

        spawner.gameObject.SetActive(false);
        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();
        foreach (Obstacle obstacle in obstacles)
        {
            obstacle.GetComponent<Collider>().enabled = false;
        }
        game_over_text.gameObject.SetActive(true);
        retry_button.gameObject.SetActive(true);
        update_high_score();
        source.pitch = pitch_controller;
        pitch_controller = 0.4f;
    }
    private void Update()
    {
        if (!enabled) return;////////////////////////////////////////////////////
        game_speed += game_speed_increase * Time.deltaTime;
        score += game_speed * Time.deltaTime;
        score_text.text = Mathf.FloorToInt(score).ToString("D5");
        source.pitch = pitch_controller;
        pitch_controller += game_speed * Time.deltaTime * 0.0001f; //можно и по другому
        colorToggle.gameObject.SetActive(true); //теперь можно менять тему во время смерти
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
