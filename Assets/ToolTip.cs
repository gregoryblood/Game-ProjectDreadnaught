using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ToolTip : MonoBehaviour
{
    [System.Serializable]
    public class Objects
    {
        public string title;
        public string description;
        public Sprite img;
    }
    [SerializeField] GameObject container;
    [SerializeField] Image image;
    [SerializeField] TMP_Text title;
    [SerializeField] TMP_Text description;

    public List<Objects> objects;
    public Dictionary<string, Objects> objectsDictionary;

    public static ToolTip Instance;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        objectsDictionary = new Dictionary<string, Objects>();
        foreach (Objects obj in objects)
        {
            objectsDictionary.Add(obj.title, obj);
        }
    }
    public void ShowToolTip(string key)
    {
        if (!objectsDictionary.ContainsKey(key))
        {
            Debug.LogWarning("Tooltip with tag " + title + " doesn't exist");
        }
        else
        {
            image.sprite = objectsDictionary[key].img;
            description.text = objectsDictionary[key].description;
            title.text = key;
        }
        Time.timeScale = 0f;
        container.SetActive(true);
    }
    public void HideToolTip()
    {
        Time.timeScale = 1f;
        PlayerControl.Instance.interactingElsewhere = false;
        container.SetActive(false);
    }
}
