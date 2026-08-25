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

    public GameObject dust_prefab; // ← Перетащи сюда префаб Dust
    private bool landed = false; // ← Чтобы не спамило
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
       
        direction +=  gravity * Time.deltaTime * Vector3.down;
        animator.SetBool("Roar", false); //не рычит
        animator.SetBool("Dead", false);


        if (character.isGrounded)
        {
            Debug.Log("На земле");
            if (landed)
            {
                SpawnDust();
            }

            direction = Vector3.down;
            animator.SetBool("Run", true);
            animator.SetBool("Jump", false);//на земле не прыгаем
            animator.SetBool("Flip", false); //не делает сальто

            // Сброс счетчика ТОЛЬКО если не прыгаем
            if (!Input.GetButtonDown("Jump"))
            {
                jump_count = 0;
            }

            if (Input.GetButtonDown("Jump") && jump_count == 0)
            {
               
                animator.SetBool("Jump", true);
                animator.SetBool("Run", false);  //нажали пробел - прыгаем
                direction = Vector3.up * jumpforce;
                jump_count = 1;
                Debug.Log("Прыжок!");

            }
        }
        else
        {
            Debug.Log("В воздухе!");
            animator.SetBool("Run", false);
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
    private void SpawnDust()
    {
        if (dust_prefab != null)
        {
            GameObject dust = Instantiate(dust_prefab);
            Destroy(dust, 1f);
        }
    }
    //столкновение
    //вместо остановки времени - сделали gameover и рестарт в менеджере
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            animator.SetBool("Run", false);
            animator.SetBool("Jump", false);
            animator.SetBool("Flip", false);
            animator.SetBool("Roar", false);

            // 
            animator.SetBool("Dead", true);
            dead = true;
            GameManager.Instance.GameOver();
        }

    }
}