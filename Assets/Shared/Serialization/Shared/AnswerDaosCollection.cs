using System;
using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.DAOs;
using UnityEngine;


namespace TopicTwister.Shared.Serialization.Shared
{
    [Serializable]
    public class AnswerDaosCollection
    {
        [SerializeField] private AnswerDaoJson[] _answers;

        public AnswerDaosCollection(AnswerDaoJson[] answers)
        {
            _answers = answers;
        }

        public List<AnswerDaoJson> Answers
        {
            get
            {
                if (_answers == null)
                {
                    return new List<AnswerDaoJson>();
                }

                return _answers.ToList();
            }
        }
    }
}
