using UnityEngine;
using UnityEngine.UI;
public class ChangeTheme : MonoBehaviour
{
    public Toggle colorToggle;

    public Color normalColor = new Color(0.839f, 0.839f, 0.839f);
    public Color pinkColor = new Color(1f, 0.75f, 0.8f);        

    private Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        colorToggle.onValueChanged.AddListener(OnToggleChanged);
        mainCamera.backgroundColor = normalColor;
    }

    void OnToggleChanged(bool isOn)
    {
        if (isOn)
        {
            mainCamera.backgroundColor = pinkColor;
        }
        else
        {
            mainCamera.backgroundColor = normalColor;
        }
    }
}