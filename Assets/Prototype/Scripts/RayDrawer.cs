using UnityEngine;

public class RayDrawer : MonoBehaviour
{
    public void OnRayInvoked(Vector3 start, Vector3 end, RaycastHit hit)
    {
        _lineRenderer.SetPosition(0, this.transform.position);
        _lineRenderer.SetPosition(1, hit.point);

        _lineRenderer.enabled = true;
        CancelInvoke();
        Invoke(nameof(OffLineRenderer), _duration);
    }
    private void OffLineRenderer()
    {
        _lineRenderer.enabled = false;
    }

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        _lineRenderer.enabled = false;
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;
    }

    [SerializeField]
    private float _duration = 0.5f;

    private LineRenderer _lineRenderer;
}
