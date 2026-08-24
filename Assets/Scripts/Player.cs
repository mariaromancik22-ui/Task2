using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController character;
    public Vector3 direction;

    public float gravity = 9.81f * 2;
    public float jumpforce = 8f;

    private int jump_count = 0;

    private Animator animator;

    public bool dead = false; //используется в менеджере для перезапуска игры
    void Awake()
    {
        character = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

    }
    void OnEnable()
    {
        direction = Vector3.zero;
        dead = false;
    }
    void Update()
    {
        if (dead)
        {
           
            return;
        }
        direction += Vector3.down * gravity * Time.deltaTime;
        animator.SetBool("Roar", false); //не рычит

        if (character.isGrounded)
        {
            direction = Vector3.down;
            animator.SetBool("Jump", false);//на земле не прыгаем
            animator.SetBool("Flip", false); //не делает сальто

            // Сброс счетчика ТОЛЬКО если не прыгаем
            if (!Input.GetButtonDown("Jump"))
            {
                jump_count = 0;
            }

            if (Input.GetButtonDown("Jump") && jump_count == 0)
            {
                animator.SetBool("Jump", true); //нажали пробел - прыгаем
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

                // ТОЛЬКО для второго прыжка (двойного)
                if (jump_count == 2)
                {
                    animator.SetBool("Flip", true); //сальто
                }
            }
        }

        character.Move(direction * Time.deltaTime);

        //рычыние на ЛКМ
        if (Input.GetMouseButton(0))
        {
            animator.SetBool("Roar", true);
        }
    }
    //столкновение
    //вместо остановки времени - сделали gameover и рестарт в менеджере
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            dead = true;
            animator.SetBool("Dead", true);
            GameManager.Instance.GameOver();
        }

    }
}