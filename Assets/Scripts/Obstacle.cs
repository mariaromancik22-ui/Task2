using UnityEngine;

public class Obstacle : MonoBehaviour
{ //раньше скорость - была константой, которую подбирали вручную для земли и препятсвтвий. теперь используем gamemanager, чтобы всё было соеденино
    private float leftEdge;
    private float speed; // Индивидуальная скорость
    public enum TypeObstacle
    {
        Cactus,
        Bird,
        BigCactus
    }
    public TypeObstacle type_obstacle;
    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 2f;
        speed = GameManager.Instance.game_speed / 5f; // скорость игры, которая изменяется во времени. делим на 5, чтобы убрать коэфициент для скриптинга бота
    }

    private void Update()
    {
        transform.position += GameManager.Instance.game_speed * Time.deltaTime * Vector3.left;

        if (transform.position.x < leftEdge)
        {
            Destroy(gameObject);
        }
    }
    public TypeObstacle GetTypeObstacle()
    {
        return type_obstacle;
    }
    public float GetSpeed()
    {
        return speed;
    }
}
