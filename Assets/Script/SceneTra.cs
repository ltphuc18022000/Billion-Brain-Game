using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTra : MonoBehaviour
{
    public GameObject Loading;
    public void LoadSceneAsync(string sceneName)
    {
        // G?i ph??ng th?c LoadSceneAsync thay vì LoadScene ?? th?c hi?n load scene b?t ??ng b?
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Thêm các ho?t ??ng b?t ??ng b? t?i ?ây
        StartCoroutine(WaitForLoad(asyncLoad));
    }

    private IEnumerator WaitForLoad(AsyncOperation asyncLoad)
    {
        Loading.SetActive(true);
        // Vòng l?p này s? ch?y trong khi scene ?ang ???c load
        while (!asyncLoad.isDone)
        {
            // Th?c hi?n các ho?t ??ng b?t ??ng b? t?i ?ây

            // T?m d?ng vòng l?p trong m?t frame ?? cho Unity ti?p t?c x? lý các ho?t ??ng khác
            yield return null;
        }

        // Scene ?ã ???c load hoàn t?t
    }
}
