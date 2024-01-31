using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class ButtonGameModeTwo : MonoBehaviour
{
    public TicTacToe TicTacToe;
    public TicTacToeSinglePlay TicTacToeSinglePlay;

    public void ShowAdFullScreen(){
        YandexGame.FullscreenShow();
    }

    public void UpdateCountsCross()
    {
        TicTacToe.PlayAgainCross();
        TicTacToeSinglePlay.PlayAgainCross();
    }

    public void UpdateCountsToe()
    {
        TicTacToe.PlayAgainToe();
        TicTacToeSinglePlay.PlayAgainToe();
    }

    public void GoToMenu()
    {
        if (TicTacToe != null && TicTacToe.gameObject.activeSelf)
        {
            TicTacToe.GoToMenu();
        }

        if (TicTacToeSinglePlay != null && TicTacToeSinglePlay.gameObject.activeSelf)
        {
            TicTacToeSinglePlay.GoToMenu();
        }
    }


}
