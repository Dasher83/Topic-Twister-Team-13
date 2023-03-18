using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "RoundAnswersData", menuName = "ScriptableObjects/RoundAnswers", order = 1)]
public class RoundAnswersScriptable : ScriptableObject
{
    [SerializeField]
   private List<RoundAnswer> roundAnswers = new List<RoundAnswer>();


}


[Serializable]
public struct RoundAnswer
{
    [SerializeField]
    private string categoryId;
    
    [SerializeField]
    private string userInput;
}