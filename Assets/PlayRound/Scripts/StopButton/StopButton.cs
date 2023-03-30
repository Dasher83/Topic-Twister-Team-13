using Assets.PlayRound.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;

namespace TopicTwister.PlayRound.Scripts.StopButton
{
    public class StopButton : MonoBehaviour
    {
        public UnityEvent InterruptRound = new UnityEvent();
        //[SerializeField]
        //private List<ISubjcet> miLista = new List<ISubjcet>();

        public void OnInterruptRound()
        {
            InterruptRound.Invoke();
            transform.gameObject.GetComponent<Button>().enabled = false;

            /*foreach (ISubjcet subjcet in ListSubjcet)
            {
                subjcet.MetodoEscuchando();
            }*/

        }
    }
}
