﻿using System;
using UnityEngine;


namespace TopicTwister.Shared.DTOs
{
    [Serializable]
    public class UserDTO
    {
        [SerializeField]
        private int _id;
        [SerializeField]
        private string _nickname;
    }
}