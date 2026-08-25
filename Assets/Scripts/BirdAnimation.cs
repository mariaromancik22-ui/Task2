using UnityEngine;
using UnityEngine.Rendering;

public class BirdAnimation : MonoBehaviour
{
    public Sprite[] sprites;
    private SpriteRenderer sprite_renderer;
    private int frame;
    private void Awake()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        Invoke(nameof(Animate), 0f);
    }
    private void Animate()
    {
        frame++;
        if (frame >= sprites.Length)
        {
            frame = 0;
        }
        if(frame >= 0 && frame < sprites.Length)
        {
            sprite_renderer.sprite = sprites[frame];
        }
        Invoke(nameof(Animate), 1f / GameManager.Instance.game_speed);
    }
   
}
