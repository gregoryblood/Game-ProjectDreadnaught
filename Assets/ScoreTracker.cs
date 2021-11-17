using UnityEngine;
using UnityEngine.UI;
public class ScoreTracker : MonoBehaviour
{
    public int capturedPlanets;
    [SerializeField] Slider scoreSlider;
    [SerializeField] float objectiveScale;
    [SerializeField] float baseRate;
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject loseScreen;
    [SerializeField] GameObject [] buildScreens;
    bool lost = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lost)
            return;
        scoreSlider.value += (baseRate + (objectiveScale * capturedPlanets)) * Time.deltaTime;
        if (scoreSlider.value >= 100f)
        {
            winScreen.SetActive(true);
            Destroy(this);
        }
    }
    public void LoseGame()
    {
        winScreen.SetActive(false);
        buildScreens[0].SetActive(false);
        buildScreens[1].SetActive(false);
        loseScreen.SetActive(true);
    }
}
