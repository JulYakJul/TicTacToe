using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGameModeTwo : MonoBehaviour
{
    public TicTacToe TicTacToe;

    public void UpdateCountsCross()
    {
        TicTacToe.PlayAgainCross();
    }

    public void UpdateCountsToe()
    {
        TicTacToe.PlayAgainToe();
    }
}
