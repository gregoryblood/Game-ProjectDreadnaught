using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    float fps;
    [SerializeField] TMP_Text fpsText;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(GetFPS), 1, 0.5f);
    }

    // Update is called once per frame
    void GetFPS()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
        fpsText.text = fps + " fps";
    }
}
