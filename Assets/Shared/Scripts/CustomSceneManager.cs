using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using TopicTwister.Shared.ScriptableObjects;


namespace TopicTwister.Shared
{
    public class CustomSceneManager : MonoBehaviour
    {
        [SerializeField]
        private LoadSceneEventScriptable _eventContainer;

    

        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            _eventContainer.LoadSceneWithoutDelay += ChangeScene;
            _eventContainer.LoadSceneWithDelay += ChangeScene;

        }

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
