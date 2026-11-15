using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CleanScene : MonoBehaviour
{
    public void Clean(int buildIndex)
    {
        StartCoroutine(CleanMem(buildIndex));
    }

    private IEnumerator CleanMem(int buildIndex)
    {
        AsyncOperation unload = SceneManager.UnloadSceneAsync(buildIndex);
        while (!unload.isDone)
        {
            yield return null;
        }

        yield return Resources.UnloadUnusedAssets();
    }
}
