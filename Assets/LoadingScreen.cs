using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadAsynchhronously(1));
    }

    // Update is called once per frame
    IEnumerator LoadAsynchhronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        while(!operation.isDone)
            yield return null;
    }
}
