using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float initial_game_speed = 5f;
    public float game_speed_increase = 0.1f;
    public float game_speed { get; private set; }
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
        NewGame();
    }
    private void NewGame()
    {
        game_speed = initial_game_speed;
    }
    private void Update()
    {
        game_speed += game_speed_increase * Time.deltaTime;
    }
}
