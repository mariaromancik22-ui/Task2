using UnityEngine;
using UnityEngine.UI;
public class ChangeTheme : MonoBehaviour
{
    public Toggle colorToggle;

    private Color normalColor = new Color(0.839f, 0.839f, 0.839f);
    private Color pinkColor = new Color(1f, 0.75f, 0.8f);        

    private Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        colorToggle.onValueChanged.AddListener(OnToggleChanged);
        mainCamera.backgroundColor = colorToggle.isOn ? normalColor : pinkColor;
    }

    void OnToggleChanged(bool isOn)
    {
        if (isOn)
        {
            mainCamera.backgroundColor = normalColor;
           
        }
        else
        {
            mainCamera.backgroundColor = pinkColor;
        }
    }
}