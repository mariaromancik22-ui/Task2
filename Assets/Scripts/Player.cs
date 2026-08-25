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

    [System.Serializable]
    public class Sounds
    {
        public AudioSource source;
        public AudioClip[] clip;
    }
    public Sounds[] sounds;
    void Play_sound(int source, int clip)
    { 
        sounds[source].source.PlayOneShot(sounds[source].clip[clip]);
    }
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
                Play_sound(0, 0);
                Debug.LogWarning("ПРЫЖОК");
                animator.SetBool("Jump", true);
                animator.SetBool("Run", false);  //нажали пробел - прыгаем
                direction = Vector3.up * jumpforce;
                jump_count = 1;
               

            }
        }
        else
        {
            Debug.Log("В воздухе!");
            animator.SetBool("Run", false);//НЕ УДАЛЯТЬ ИНАЧЕ АНИМАЦИЯ СМЕРТИ ОСТАНЕТСЯ ПОСЛЕ РЕСТАРТА
            if (Input.GetButtonDown("Jump") && jump_count < 2)
            {
                Play_sound(0, 0);
                Debug.LogWarning("ПРЫЖОК");
                animator.SetBool("Jump", true); //починен пропуск анимации прыжка!
                animator.SetBool("Run", false); 
                direction = Vector3.up * jumpforce;
                jump_count++;

                // ТОЛЬКО для второго прыжка (двойного)
                if (jump_count == 2)
                {
                 
                    animator.SetBool("Flip", true); //сальто
                    sounds[0].source.Stop();
                    Play_sound(1, 0);
                }
               
            }
        }

        character.Move(direction * Time.deltaTime);

        //рычыние на ЛКМ // изменено рычание на R
        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetBool("Roar", true);
            Play_sound(2, 0);
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            animator.SetBool("Roar", false);
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
            Play_sound(3, 0);
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