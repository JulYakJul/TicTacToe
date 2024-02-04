using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;
using TMPro;

public class ButtonGameModeTwo : MonoBehaviour
{
    public TicTacToe TicTacToe;
    public TicTacToeSinglePlay TicTacToeSinglePlay;
    public TextBlinkAndDisappear RulesText;

    public void ShowAdFullScreen(){
        YandexGame.FullscreenShow();
    }

    public void UpdateCountsCross()
    {
        if (TicTacToe != null && TicTacToe.gameObject.activeSelf)
        {
            TicTacToe.PlayAgainCross();
        }

        if (TicTacToeSinglePlay != null && TicTacToeSinglePlay.gameObject.activeSelf)
        {
            TicTacToeSinglePlay.PlayAgainCross();
        }
    }

    public void UpdateCountsToe()
    {
        if (TicTacToe != null && TicTacToe.gameObject.activeSelf)
        {
            TicTacToe.PlayAgainToe();
        }

        if (TicTacToeSinglePlay != null && TicTacToeSinglePlay.gameObject.activeSelf)
        {
            TicTacToeSinglePlay.PlayAgainToe();
        }
    }

    public void GoToMenu()
    {  
        if (TicTacToeSinglePlay.isSingleMode == true){
            TicTacToeSinglePlay.GoToMenu();
            TicTacToeSinglePlay.isSingleMode = false;
        } else if (TicTacToe.isTwoMode == true){
            TicTacToe.GoToMenu();   
            TicTacToe.isTwoMode = false;
        }
    }

    public void StartGameTwoMode(){
        Debug.Log("инициализация сетки для игры вдвоем");
        TicTacToe.InitializeGrid();
        TicTacToe.isTwoMode = true;
        RulesText.PlayBlinkAndDisappear();
    }

    public void StartGameSingleMode(){
        Debug.Log("инициализация сетки для игры с компьютером");
        TicTacToeSinglePlay.InitializeGrid();
        TicTacToeSinglePlay.isSingleMode = true;
        RulesText.PlayBlinkAndDisappear();
    }
}
