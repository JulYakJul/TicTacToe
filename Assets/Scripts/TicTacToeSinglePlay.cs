using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic; 

public class TicTacToeSinglePlay : MonoBehaviour
{
    public Image crossPrefab;
    public Image toePrefab;
    
    public Image player1Image;
    public Image player2Image; 

    public Sprite crossSprite1;
    public Sprite crossSprite2;
    public Sprite toeSprite1;
    public Sprite toeSprite2;

    private Cell[,] grid; 
    public ParticleSystem particleSystemCrossPrefab;
    public ParticleSystem particleSystemToePrefab;

    public Animator CrossAnimator;
    public Animator ToeAnimator;

    public LanguageManager LanguageManager;

    public AnimationClip toeWinAnimationClip;
    public AnimationClip crossWinAnimationClip;

    private bool isPlayerXTurn = true;
    private bool isCross;
    private bool isWin;
    public bool isSingleMode = false;

    public int crossesWinCount = 0;
    public int toesWinCount = 0;

    public TextMeshProUGUI CrossText;
    public TextMeshProUGUI ToeText;
    public TextMeshProUGUI GameState;

    public Button playAgainButtonCross;
    public Button playAgainButtonToe;

    private bool isVsComputer = true;

    private bool isCrossTurn;

    void Start()
    {
        if (playAgainButtonCross != null)
        {
            playAgainButtonCross.onClick.AddListener(PlayAgainCross);
        }

        if (playAgainButtonToe != null)
        {
            playAgainButtonToe.onClick.AddListener(PlayAgainToe);
        }

        isCrossTurn = true;
    }

    void Update(){
        if (toesWinCount >= 3 && toesWinCount > crossesWinCount)
        {
            toesWinCount = 0;
            crossesWinCount = 0;
            ToeAnimator.SetTrigger("isToeWin");
        }
        else if (crossesWinCount >= 3 && crossesWinCount > toesWinCount)
        {
            toesWinCount = 0;
            crossesWinCount = 0;
            CrossAnimator.SetTrigger("isCrossWin");
        }
    }

    public void InitializeGrid()
    {
        grid = new Cell[3, 3];

        GridLayoutGroup gridLayout = GetComponent<GridLayoutGroup>();

        isPlayerXTurn = true;

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                Cell cell = new Cell(row, col);
                grid[row, col] = cell;

                Button button = Instantiate(ButtonPrefab(), transform);
                cell.SetButton(button);
                button.onClick.AddListener(() => OnCellClick(cell));

                cell.Button.image.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            }
        }
    }

    public void PlayAgainToe()
    {
        toesWinCount = 0;
        ToeText.text = toesWinCount.ToString();

        crossesWinCount = 0;
        CrossText.text = crossesWinCount.ToString();

        isWin = false;

        if (ToeAnimator != null)
        {
            ToeAnimator.SetTrigger("PlayAgain");
        }
    }

    public void PlayAgainCross()
    {
        toesWinCount = 0;
        ToeText.text = toesWinCount.ToString();

        crossesWinCount = 0;
        CrossText.text = crossesWinCount.ToString();

        isWin = false;

        if (CrossAnimator != null)
        {
            CrossAnimator.SetTrigger("PlayAgain");
        }
    }

    void OnCellClick(Cell cell)
    {
        if (!IsGameInProgress())
        {
            return;
        }

        if (cell.IsEmpty)
        {
            Image newSymbol;

            if (isPlayerXTurn)
            {
                newSymbol = Instantiate(crossPrefab);
                cell.SetSymbol(newSymbol, SymbolType.Cross);
            }
            else
            {
                newSymbol = Instantiate(toePrefab);
                cell.SetSymbol(newSymbol, SymbolType.Toe);
            }

            newSymbol.transform.SetParent(cell.Button.transform, false);

            cell.Button.image.sprite = newSymbol.sprite;
            grid[cell.Row, cell.Col] = cell;

            cell.Button.image.color = Color.white;

            CheckForWinner(cell.Row, cell.Col);

            ParticleSystem particleSystemPrefab = isPlayerXTurn ? particleSystemCrossPrefab : particleSystemToePrefab;

            ActivateParticleEffect(cell.Button.transform.position, particleSystemPrefab);

            isPlayerXTurn = !isPlayerXTurn;

            player1Image.sprite = isPlayerXTurn ? crossSprite2 : toeSprite2;
            player2Image.sprite = isPlayerXTurn ? toeSprite2 : crossSprite2;

            if (IsGridFull() && !isWin)
            {
                if (LanguageManager.RussianLanguage == true){
                    StartCoroutine(DisplayGameStateFor3Seconds("Ничья!"));
                } else if (LanguageManager.EnglishLanguage == true){
                    StartCoroutine(DisplayGameStateFor3Seconds("Tie!"));
                } else if (LanguageManager.TurkishLanguage == true){
                    StartCoroutine(DisplayGameStateFor3Seconds("Kravat!"));
                }

                StartCoroutine(EndGame());
            }

            if (isVsComputer && !isWin && !isPlayerXTurn)
            {
                MakeComputerMove();
            }
        }
    }


    void MakeComputerMove()
    {
        Cell winningMove = FindWinningMove(SymbolType.Toe);
        if (winningMove != null)
        {
            MakeMoveAtCell(winningMove, SymbolType.Toe);
            return;
        }

        Cell blockingMove = FindWinningMove(SymbolType.Cross);
        if (blockingMove != null)
        {
            MakeMoveAtCell(blockingMove, SymbolType.Toe);
            return;
        }

        Cell randomMove = FindRandomEmptyCell();
        if (randomMove != null)
        {
            MakeMoveAtCell(randomMove, SymbolType.Toe);
            return;
        }
    }

    void MakeMoveAtCell(Cell cell, SymbolType symbolType)
    {
        Image newSymbol = Instantiate(toePrefab);

        newSymbol.transform.SetParent(cell.Button.transform, false);
        cell.SetSymbol(newSymbol, symbolType);

        cell.Button.image.sprite = newSymbol.sprite;
        grid[cell.Row, cell.Col] = cell;

        cell.Button.image.color = Color.white;

        CheckForWinner(cell.Row, cell.Col);

        ParticleSystem particleSystemPrefab = symbolType == SymbolType.Cross ? particleSystemCrossPrefab : particleSystemToePrefab;

        ActivateParticleEffect(cell.Button.transform.position, particleSystemPrefab);

        isPlayerXTurn = !isPlayerXTurn;
    }

    Cell FindWinningMove(SymbolType symbolType)
    {
        for (int row = 0; row < 3; row++)
        {
            int emptyCount = 0;
            Cell emptyCell = null;

            for (int col = 0; col < 3; col++)
            {
                if (grid[row, col].SymbolType == SymbolType.None)
                {
                    emptyCount++;
                    emptyCell = grid[row, col];
                }
                else if (grid[row, col].SymbolType != symbolType)
                {
                    emptyCount = 0;
                    emptyCell = null;
                    break;
                }
            }

            if (emptyCount == 1)
            {
                return emptyCell;
            }
        }

        for (int col = 0; col < 3; col++)
        {
            int emptyCount = 0;
            Cell emptyCell = null;

            for (int row = 0; row < 3; row++)
            {
                if (grid[row, col].SymbolType == SymbolType.None)
                {
                    emptyCount++;
                    emptyCell = grid[row, col];
                }
                else if (grid[row, col].SymbolType != symbolType)
                {
                    emptyCount = 0;
                    emptyCell = null;
                    break;
                }
            }

            if (emptyCount == 1)
            {
                return emptyCell;
            }
        }

        int diagonal1Count = 0;
        Cell diagonal1Cell = null;

        int diagonal2Count = 0;
        Cell diagonal2Cell = null;

        for (int i = 0; i < 3; i++)
        {
            if (grid[i, i].SymbolType == SymbolType.None)
            {
                diagonal1Count++;
                diagonal1Cell = grid[i, i];
            }
            else if (grid[i, i].SymbolType != symbolType)
            {
                diagonal1Count = 0;
                diagonal1Cell = null;
                break;
            }

            if (grid[i, 2 - i].SymbolType == SymbolType.None)
            {
                diagonal2Count++;
                diagonal2Cell = grid[i, 2 - i];
            }
            else if (grid[i, 2 - i].SymbolType != symbolType)
            {
                diagonal2Count = 0;
                diagonal2Cell = null;
                break;
            }
        }

        if (diagonal1Count == 1)
        {
            return diagonal1Cell;
        }

        if (diagonal2Count == 1)
        {
            return diagonal2Cell;
        }

        return null;
    }


    Cell FindRandomEmptyCell()
    {
        List<Cell> emptyCells = new List<Cell>();

        foreach (Cell cell in grid)
        {
            if (cell.IsEmpty)
            {
                emptyCells.Add(cell);
            }
        }

        if (emptyCells.Count > 0)
        {
            int randomIndex = Random.Range(0, emptyCells.Count);
            return emptyCells[randomIndex];
        }

        return null;
    }

    bool IsGameInProgress()
    {
        return GameState.text == "";
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1f);
        isWin = false;
        StartCoroutine(ResetGrid());
    }

    public void GoToMenu(){
        Debug.Log("Ресет сетка игры с компьютером");
        foreach (Cell cell in grid)
        {
            Destroy(cell.Button.gameObject);
        }

        isWin = false;
        isPlayerXTurn = true;
    }

    bool IsGridFull()
    {
        foreach (Cell cell in grid)
        {
            if (cell.IsEmpty)
            {
                return false;
            }
        }
        return true;
    }

    void ActivateParticleEffect(Vector3 position, ParticleSystem particleSystemPrefab)
    {
        ParticleSystem particleSystem = Instantiate(particleSystemPrefab, position, Quaternion.identity);
        Destroy(particleSystem.gameObject, particleSystem.main.duration);
    }

    void ActivateWinAnimation(bool isCross)
    {
        if (CrossAnimator != null && ToeAnimator != null)
        {
            if (isCross && crossWinAnimationClip != null)
            {
                CrossAnimator.SetTrigger("isCrossWin");
            }
            else if (!isCross && toeWinAnimationClip != null)
            {
                ToeAnimator.SetTrigger("isToeWin");
            }
        }
    }

    Button ButtonPrefab()
    {
        GameObject buttonObject = new GameObject("Button");
        buttonObject.AddComponent<RectTransform>();
        Button button = buttonObject.AddComponent<Button>();
        Image image = buttonObject.AddComponent<Image>();
        button.targetGraphic = image;
        return button;
    }

    IEnumerator DisplayGameStateFor3Seconds(string message)
    {
        GameState.text = message;
        yield return new WaitForSeconds(2f);
        GameState.text = "";
    }

    void CheckForWinner(int lastRow, int lastCol)
    {
        if (grid[lastRow, 0].SymbolType != SymbolType.None && grid[lastRow, 0].SymbolType == grid[lastRow, 1].SymbolType && grid[lastRow, 1].SymbolType == grid[lastRow, 2].SymbolType)
        {
            if (grid[lastRow, 0].SymbolType == SymbolType.Cross)
            {
                if (LanguageManager.RussianLanguage == true){
                    StartCoroutine(DisplayGameStateFor3Seconds("Крестики выиграли в строке " + (lastRow + 1) + "!"));
                } else if (LanguageManager.EnglishLanguage == true){
                    StartCoroutine(DisplayGameStateFor3Seconds("Victory! The X's won the row " + (lastRow + 1) + "!"));
                } else if (LanguageManager.TurkishLanguage == true){
                    StartCoroutine(DisplayGameStateFor3Seconds("Zafer! Haçlar üst üste kazandı " + (lastRow + 1) + "!"));
                }
                
                crossesWinCount++;
                CrossText.text = crossesWinCount.ToString();
                StartCoroutine(ResetGrid());
                isWin = true;
            }
            else if (grid[lastRow, 0].SymbolType == SymbolType.Toe)
            {
                if (LanguageManager.RussianLanguage == true){
                    StartCoroutine(DisplayGameStateFor3Seconds("Нолики выиграли в строке " + (lastRow + 1) + "!"));
                } else if (LanguageManager.EnglishLanguage == true){
                    StartCoroutine(DisplayGameStateFor3Seconds("Victory! The Noles won in a row " + (lastRow + 1) + "!"));
                } else if (LanguageManager.TurkishLanguage == true){
                    StartCoroutine(DisplayGameStateFor3Seconds("Zafer! Noles üst üste kazandı " + (lastRow + 1) + "!"));
                }

                toesWinCount++;
                ToeText.text = toesWinCount.ToString();
                StartCoroutine(ResetGrid());
                isWin = true;
            }
        }

        if (grid[0, lastCol].SymbolType != SymbolType.None && grid[0, lastCol].SymbolType == grid[1, lastCol].SymbolType && grid[1, lastCol].SymbolType == grid[2, lastCol].SymbolType)
        {
            if (grid[0, lastCol].SymbolType == SymbolType.Cross)
            {
                if (LanguageManager.RussianLanguage == true){
                    StartCoroutine(DisplayGameStateFor3Seconds("Крестики выиграли в колонке " + (lastCol + 1) + "!"));
                } else if (LanguageManager.EnglishLanguage == true){
                    StartCoroutine(DisplayGameStateFor3Seconds("Victory! The X's won the column " + (lastRow + 1) + "!"));
                } else if (LanguageManager.TurkishLanguage == true){
                    StartCoroutine(DisplayGameStateFor3Seconds("Zafer! X'ler sütunu kazandı " + (lastRow + 1) + "!"));
                }

                crossesWinCount++;
                CrossText.text = crossesWinCount.ToString();
                StartCoroutine(ResetGrid());
                isWin = true;
            }
            else if (grid[0, lastCol].SymbolType == SymbolType.Toe)
            {
                if (LanguageManager.RussianLanguage == true){
                    StartCoroutine(DisplayGameStateFor3Seconds("Нолики выиграли в колонке " + (lastCol + 1) + "!"));
                } else if (LanguageManager.EnglishLanguage == true){
                    StartCoroutine(DisplayGameStateFor3Seconds("Victory! The Noles won the column " + (lastRow + 1) + "!"));
                } else if (LanguageManager.TurkishLanguage == true){
                    StartCoroutine(DisplayGameStateFor3Seconds("Zafer! Noles sütunu kazandı " + (lastRow + 1) + "!"));
                }

                toesWinCount++;
                ToeText.text = toesWinCount.ToString();
                StartCoroutine(ResetGrid());
                isWin = true;
            }
        }

        if ((lastRow == lastCol || lastRow + lastCol == 2) &&
            (grid[0, 0].SymbolType != SymbolType.None && grid[0, 0].SymbolType == grid[1, 1].SymbolType && grid[1, 1].SymbolType == grid[2, 2].SymbolType ||
            grid[0, 2].SymbolType != SymbolType.None && grid[0, 2].SymbolType == grid[1, 1].SymbolType && grid[1, 1].SymbolType == grid[2, 0].SymbolType))
        {
            if (grid[1, 1].SymbolType == SymbolType.Cross)
            {
                if (LanguageManager.RussianLanguage == true){  
                    StartCoroutine(DisplayGameStateFor3Seconds("Крестики выиграли по диагонали!"));
                } else if (LanguageManager.EnglishLanguage == true){
                    StartCoroutine(DisplayGameStateFor3Seconds("Victory! The X's have won on the diagonal!"));
                } else if (LanguageManager.TurkishLanguage == true){
                    StartCoroutine(DisplayGameStateFor3Seconds("Zafer! Çaprazlar çapraz olarak kazandı!"));
                }
                
                crossesWinCount++;
                CrossText.text = crossesWinCount.ToString();
                StartCoroutine(ResetGrid());
                isWin = true;
            }
            else if (grid[1, 1].SymbolType == SymbolType.Toe)
            {
                if (LanguageManager.RussianLanguage == true){  
                    StartCoroutine(DisplayGameStateFor3Seconds("Нолики выиграли по диагонали!"));
                } else if (LanguageManager.EnglishLanguage == true){
                    StartCoroutine(DisplayGameStateFor3Seconds("Victory! The Noles have won on the diagonal!"));
                } else if (LanguageManager.TurkishLanguage == true){
                    StartCoroutine(DisplayGameStateFor3Seconds("Zafer! Noles çapraz olarak kazandı!"));
                }

                toesWinCount++;
                ToeText.text = toesWinCount.ToString();
                StartCoroutine(ResetGrid());
                isWin = true;
            }
        }
        
        if (toesWinCount == 3 && toesWinCount > crossesWinCount){
            ToeAnimator.SetTrigger("isToeWin");
        } else if (crossesWinCount == 3 && crossesWinCount > toesWinCount) {
            CrossAnimator.SetTrigger("isCrossWin");
        }
    }

    IEnumerator ResetGrid()
    {
        yield return new WaitForSeconds(2f);
        foreach (Cell cell in grid)
        {
            Destroy(cell.Button.gameObject);
        }

        isWin = false;
        isPlayerXTurn = true;

        InitializeGrid();
    }

    public enum SymbolType
    {
        None,
        Cross,
        Toe
    }

    public class Cell
    {
        public int Row { get; private set; }
        public int Col { get; private set; }
        public Button Button { get; private set; }
        public Image Symbol { get; private set; }
        public SymbolType SymbolType { get; private set; }

        public bool IsEmpty => SymbolType == SymbolType.None;

        public Cell(int row, int col)
        {
            Row = row;
            Col = col;
            SymbolType = SymbolType.None;
        }

        public void SetSymbol(Image symbol, SymbolType symbolType)
        {
            Symbol = symbol;
            SymbolType = symbolType;
        }

        public void SetSymbolType(SymbolType symbolType)
        {
            SymbolType = symbolType;
        }

        public void SetButton(Button button)
        {
            Button = button;
        }
    }
}
