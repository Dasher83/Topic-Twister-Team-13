using System.Collections.Generic;
using TopicTwister.Shared.Structs;
using UnityEngine;

[CreateAssetMenu(fileName = "RoundAnswersData", menuName = "ScriptableObjects/RoundAnswers", order = 1)]
public class RoundAnswersScriptable : ScriptableObject
{
    [SerializeField] private List<RoundAnswer> _roundAnswers = new List<RoundAnswer>();

    public void AddAnswers(RoundAnswer[] roundAnswers)
    {
        foreach (RoundAnswer roundAnswer in roundAnswers)
        {
            _roundAnswers.Add(roundAnswer);
        }
    }

    public void ClearAnswers()
    {
        _roundAnswers.Clear();
    }
}