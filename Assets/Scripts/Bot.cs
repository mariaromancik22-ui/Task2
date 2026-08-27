using UnityEngine;
using System.Collections;
using System.Buffers;

public class Bot : MonoBehaviour
    //в срипте использовался другой тип рейкаста - для 3д,
    //потому что все коллайдеры сделаны типа 3д, а не 2д.
{
    Player player;
    public enum TypeObject
    {
        Nothing,
        Cactus,
        Bird,
        Ground,
        BigCactus//решило баг в принятии решения, джамп  или даблджамп? можно было исправить логику без этого?
    }

    TypeObject object_down;
    TypeObject object_right;
    TypeObject object_up_right;

    private float distance_right;
    private float distance_up_right;

    private float speed_right;
    private float speed_up_right;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }
    void FixedUpdate()
    {
     
        player.direction += player.gravity * Time.deltaTime * Vector3.down;

        // вниз
        RaycastHit hit_down;
        if (Physics.Raycast(transform.position, Vector3.down, out hit_down))
        {
            var enemy = hit_down.collider.gameObject.GetComponent<Obstacle>();
            if (enemy != null)
            {
                object_down = enemy.GetTypeObstacle() == Obstacle.TypeObstacle.Cactus ? TypeObject.Cactus : TypeObject.Bird;
            }
            else {

                if(Vector2.Distance(transform.position, hit_down.point) < 0.5f)
                {
                    object_down = TypeObject.Ground;
                }
                else
                {
                    object_down = TypeObject.Nothing;
                }
            }
        }
        else
        {
            object_down = TypeObject.Nothing;
        }

        // вправо 
        RaycastHit hit_right;
        if (Physics.Raycast(transform.position, Vector3.right, out hit_right, 6f)) //бесконечная дальность мешала выбору джамп или даблджамп?
        {
            var enemy = hit_right.collider.gameObject.GetComponent<Obstacle>();
            if (enemy != null)
            {
                object_right = enemy.GetTypeObstacle() == Obstacle.TypeObstacle.BigCactus ? TypeObject.BigCactus : TypeObject.Cactus;
                distance_right = Vector2.Distance( transform.position, enemy.transform.position);
                speed_right = enemy.GetSpeed();
            }
            else
            {
                object_right = TypeObject.Nothing;
            }
        }
        else
        {
            object_right = TypeObject.Nothing;
        }

        //вверх вправо 
        RaycastHit hit_up_right;
        if (Physics.Raycast(transform.position + Vector3.up * 0.7f, Vector3.right, out hit_up_right, 6f))
        {
            var enemy = hit_up_right.collider.gameObject.GetComponent<Obstacle>();
            if (enemy != null)
            {
                object_up_right = enemy.GetTypeObstacle() == Obstacle.TypeObstacle.BigCactus ? TypeObject.BigCactus : TypeObject.Bird;
                distance_up_right = Vector2.Distance(transform.position, enemy.transform.position);
                speed_up_right = enemy.GetSpeed();
            }
            else
            {
                object_up_right = TypeObject.Nothing;
            }
        }
        else
        {
            object_up_right = TypeObject.Nothing;
        }
        Debug.DrawRay(transform.position, Vector3.down * 10f, Color.red);//юзалось для проверки в начале
       
        player.character.Move(player.direction * Time.deltaTime);
    }

    int time;
    public float reactin_distance_right = 1.4f;
    public float reactin_distance_up_right = 2f;

    //дино не всегда реагирует на приближающийся объет, хоть и видит его.
    //возможная причина - нагруженность программы
    void Update()
    {
       time = (time + 1) % 2;// вот что может нагружать. если часто обновлять кадр, но ноут может не вывести такие расчеты?
       if (time == 0)
        {
            if (object_down == TypeObject.Ground)
            {
               
                if (object_right == TypeObject.Cactus && distance_right * speed_right < reactin_distance_right)
                {
                    player.Jump(); 
                }
                if (object_up_right == TypeObject.BigCactus && distance_up_right * speed_up_right < reactin_distance_up_right)
                {
                    StartCoroutine(JumpWithDelay()); 
                }
            }
        }
    }

    //двойной прыжок очень сложно настраивался. никак не получилось поставить, чтобы второй прыжок совершался через какое-то время
    //дино не мог перепрыгнуть высокие кактусы
    //поэтому дино всегда высоко прыгает и иногда делает сальто
    IEnumerator JumpWithDelay()//по высоте тоже самое что и jump, потому что WaitForEndOfFrame - очень маленькое значение
    {
        player.Jump();
        yield return new WaitForEndOfFrame();
        player.DoubleJump();
    }
    //также интересно: по какойто причине дино начаал проваливаться сквозь землю каждый запуск игры.
    //причина - лаг
    //причина лагов??? щадящий режим ноута?
    //наверное, это начало случаться изза весы программы
    private void OnDrawGizmos()
    {
        switch (object_down)
        {
            case TypeObject.Nothing: Gizmos.color = Color.blue; break;
            case TypeObject.Ground: Gizmos.color = Color.green; break;
            case TypeObject.Cactus: Gizmos.color = Color.yellow; break;
            case TypeObject.BigCactus: Gizmos.color = Color.brown; break;
            case TypeObject.Bird: Gizmos.color = Color.red; break;
        }
        Gizmos.DrawSphere(transform.position + Vector3.down, 0.25f);
        switch (object_right)
        {
            case TypeObject.Nothing: Gizmos.color = Color.blue; break;
            case TypeObject.Cactus: Gizmos.color = Color.yellow; break;
            case TypeObject.Bird: Gizmos.color = Color.red; break;
            case TypeObject.BigCactus: Gizmos.color = Color.brown; break;
        }
        Gizmos.DrawSphere(transform.position + Vector3.right, 0.25f);
        switch (object_up_right)
        {
            case TypeObject.Nothing: Gizmos.color = Color.blue; break;
            case TypeObject.Cactus: Gizmos.color = Color.yellow; break;
            case TypeObject.Bird: Gizmos.color = Color.red; break;
            case TypeObject.BigCactus: Gizmos.color = Color.brown; break;
        }
        Gizmos.DrawSphere(transform.position + Vector3.up * 1f + Vector3.right, 0.25f);
    }
}


