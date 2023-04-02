using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TopicTwister.PlayRound.Scripts.StopButton
{
    public class StopButton : MonoBehaviour
    {
        public UnityEvent InterruptRound = new UnityEvent();
        public void OnInterruptRound()
        {
            InterruptRound.Invoke();
            transform.gameObject.GetComponent<Button>().enabled = false;
        }
    }
}
