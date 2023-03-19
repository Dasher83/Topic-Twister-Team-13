using UnityEngine;
using UnityEngine.Events;


namespace TopicTwister.PlayRound.Scripts.StopButton
{
    public class StopButton : MonoBehaviour
    {
        public UnityEvent InterruptRound = new UnityEvent();

        public void OnInterruptRound()
        {
            InterruptRound.Invoke();
        }
    }
}
