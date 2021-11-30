
using UnityEngine;

public class RangeDrawer : MonoBehaviour
{
    float ThetaScale = 0.01f;
    public float radius = 5f;
    private int Size;
    private LineRenderer LineDrawer;
    private float Theta = 0f;

    void Awake()
    {
        LineDrawer = GetComponent<LineRenderer>();
    }
    private void OnEnable()
    {
        if (LineDrawer)
        {
            LineDrawer.enabled = true;
            Size = (int)((1f / ThetaScale) + 1f);
            LineDrawer.positionCount = Size;
        }
    }
    private void OnDisable()
    {
        if (LineDrawer)
            LineDrawer.enabled = false;
    }
    void Update()
    {
        Theta = 0f;

        for (int i = 0; i < Size; i++)
        {
            Theta += (2.0f * Mathf.PI * ThetaScale);
            float x = radius * Mathf.Cos(Theta) + transform.position.x;
            float y = radius * Mathf.Sin(Theta) + transform.position.y; ;
            LineDrawer.SetPosition(i, new Vector3(x, y, 0));
        }
    }
}
