using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController character;
    public Vector3 direction;

    public float gravity = 9.81f * 2;
    public float jumpforce = 8f;

    private int jump_count = 0;

    private Animator animator;

    public bool dead = false; //используется в менеджере для перезапуска игры

    public GameObject dust_prefab; //не использовалось изза бага
    private bool landed = false; 

    private Coroutine roarCoroutine;

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
    public void Jump()
    {
        if (dead) return;

        //прыжок с земли
        if (character.isGrounded)
        {
            Play_sound(0, 0);
            Debug.Log("ПРЫЖОК");
            direction = Vector3.up * jumpforce;
            animator.SetBool("Jump", true);
            animator.SetBool("Run", false);
            jump_count = 1;
        }
    }

    public void DoubleJump()
    {
        if (dead) return;
        //двойной прыжок в воздухе
        if (!character.isGrounded && jump_count < 2)
        {
            Play_sound(0, 0);
            Debug.Log("ДВОЙНОЙ ПРЫЖОК");
            direction = Vector3.up * jumpforce;
            animator.SetBool("Jump", true);
            animator.SetBool("Run", false);
            jump_count++;
            if (jump_count == 2)
            {
                animator.SetBool("Flip", true);
                sounds[0].source.Stop();
                Play_sound(1, 0);
            }
        }
    }
    void Update()
    {
        if (dead) return;

        direction += gravity * Time.deltaTime * Vector3.down;
        animator.SetBool("Dead", false);

        if (character.isGrounded)
        {
            //Debug.Log("На земле");
            direction = Vector3.down;
            animator.SetBool("Run", true);
            animator.SetBool("Jump", false);
            animator.SetBool("Flip", false);
            if (!Input.GetButtonDown("Jump"))
            {
                jump_count = 0;
            }
            
            if (Input.GetButtonDown("Jump") && jump_count == 0)
            {
                Jump(); 
            }
        }
        else
        {
            //Debug.Log("В воздухе!");
            animator.SetBool("Run", false);
            if (Input.GetButtonDown("Jump") && jump_count < 2)
            {
                DoubleJump();
            }
        }

        character.Move(direction * Time.deltaTime);

        //рычыние на ЛКМ --> изменено рычание на R изза бага (работало через раз)
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (roarCoroutine != null)
            {
                StopCoroutine(roarCoroutine);
                animator.SetBool("Roar", false);
                sounds[2].source.Stop(); 
            }
            roarCoroutine = StartCoroutine(PlayRoar());
        }
    }
    private System.Collections.IEnumerator PlayRoar()
    {
        animator.SetBool("Roar", true);
        Play_sound(2, 0);
        yield return new WaitForSeconds(sounds[2].clip[0].length);
        //анимация рыка = продолжительность звука рыка
        animator.SetBool("Roar", false);
        roarCoroutine = null;
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
    // остановки времени --> gameover и рестарт в менеджере
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