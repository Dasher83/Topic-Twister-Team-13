using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShuffleLetterPresenter
{
    void GetRandomLetter();

    void ShowLetter(string letter);
}
