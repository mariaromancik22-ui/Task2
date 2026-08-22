using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController character;
    public Vector3 direction;

    public float gravity = 9.81f * 2;
    public float jumpforce = 8f;

    private int jump_count = 0;
    void Awake()
    {
        character = GetComponent<CharacterController>();

    }
    void OnEnable()
    {
        direction = Vector3.zero;
    }
    void Update()
    {
        direction += Vector3.down * gravity * Time.deltaTime;

        if (character.isGrounded)
        {
            jump_count = 0;
            direction = Vector3.down;
            if (Input.GetButton("Jump"))
            {
                direction = Vector3.up * jumpforce;
                jump_count = 1;
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump") && jump_count < 2)
            {
                direction = Vector3.up * jumpforce;
                jump_count++;
            }
        }
        character.Move(direction * Time.deltaTime);
    }
    //столкновение
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Time.timeScale = 0f;
        }
    }
}