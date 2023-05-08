using TopicTwister.PlayTurn.Shared.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;


namespace TopicTwister.PlayTurn.UI
{
    public class StopButton : MonoBehaviour
    {
        [SerializeField] private InterruptTurnEventScriptable _eventContainer;

        public void OnInterruptTurn()
        {
            _eventContainer.InterruptTurn?.Invoke();
            transform.gameObject.GetComponent<Button>().enabled = false;
        }
    }
}
