using UnityEngine;

public class PlayerHUDController : MonoBehaviour
{
    [SerializeField]
    private GameObject _hudUIPrefab;

    public GameObject UIInstance { get; private set; }

    private void Start()
    {
        DoCreateUIInstance();
    }

    public void CreateUIInstance()
    {
        if (!UIInstance)
        {
            DoCreateUIInstance();
        }
    }
    private void DoCreateUIInstance()
    {
        if (_hudUIPrefab)
        {
            Canvas uiCanvas = GameObject.FindAnyObjectByType<Canvas>();

            UIInstance = Instantiate(_hudUIPrefab, uiCanvas.transform);
        }
    }

    public void RemoveUIInstance()
    {
        if (UIInstance)
        {
            Destroy(UIInstance);
        }
    }
}
