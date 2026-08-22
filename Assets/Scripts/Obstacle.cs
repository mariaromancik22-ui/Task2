using UnityEngine;

public class Obstacle : MonoBehaviour
{ //раньше скорость - была константой, которую подбирали вручную для земли и препятсвтвий. теперь используем gamemanager, чтобы всё было соеденино
    private float leftEdge;
    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 2f;
    }

    private void Update()
    {
        transform.position += GameManager.Instance.game_speed * Time.deltaTime * Vector3.left;

        if (transform.position.x < leftEdge) {
            Destroy(gameObject);
        }
    }

}
