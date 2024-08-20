using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTra : MonoBehaviour
{
    public GameObject Loading;
    public void LoadSceneAsync(string sceneName)
    {
        // G?i ph??ng th?c LoadSceneAsync thay v� LoadScene ?? th?c hi?n load scene b?t ??ng b?
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Th�m c�c ho?t ??ng b?t ??ng b? t?i ?�y
        StartCoroutine(WaitForLoad(asyncLoad));
    }

    private IEnumerator WaitForLoad(AsyncOperation asyncLoad)
    {
        Loading.SetActive(true);
        // V�ng l?p n�y s? ch?y trong khi scene ?ang ???c load
        while (!asyncLoad.isDone)
        {
            // Th?c hi?n c�c ho?t ??ng b?t ??ng b? t?i ?�y

            // T?m d?ng v�ng l?p trong m?t frame ?? cho Unity ti?p t?c x? l� c�c ho?t ??ng kh�c
            yield return null;
        }

        // Scene ?� ???c load ho�n t?t
    }
}
