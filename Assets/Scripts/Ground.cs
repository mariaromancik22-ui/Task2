using UnityEngine;
[RequireComponent(typeof(MeshRenderer))]
public class Ground : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private MeshRenderer mesh_renderer;
    private void Awake()
    {
        mesh_renderer = GetComponent<MeshRenderer>();
    }
    //раньше скорость - была константой, которую подбирали вручную для земли и препятсвтвий. теперь используем gamemanager, чтобы всё было соеденино
 
    private void Update()
    {
        float speed = GameManager.Instance.game_speed / transform.localScale.x;
        mesh_renderer.material.mainTextureOffset += Vector2.right * Time.deltaTime * speed;
    }
}
