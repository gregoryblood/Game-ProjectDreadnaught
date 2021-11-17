using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    float fps;
    float lowFPS = 1000;
    [SerializeField] TMP_Text fpsText;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(GetFPS), 1, 1f);
    }
    private void Update()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);

        if (fps < lowFPS)
        {
            lowFPS = fps;
        }
    }
    // Update is called once per frame
    void GetFPS()
    {
        fpsText.text = lowFPS.ToString();
        lowFPS = 1000;
    }
}
