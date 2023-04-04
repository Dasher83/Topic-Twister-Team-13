using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;


namespace TopicTwister.Shared
{
    public class CustomSceneManager : MonoBehaviour
    {
        public void ChangeScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void ChangeScene(string sceneName, float transitionTime)
        {
            StartCoroutine(LoadSceneWithTransition(sceneName, transitionTime));
        }

        private IEnumerator LoadSceneWithTransition(string sceneName, float transitionTime)
        {
            yield return new WaitForSeconds(transitionTime);
            SceneManager.LoadScene(sceneName);
        }
    }
}