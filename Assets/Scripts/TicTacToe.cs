using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TicTacToe : MonoBehaviour
{
    public Image crossPrefab;
    public Image toePrefab;

    private Cell[,] grid; 
    public ParticleSystem particleSystemCrossPrefab;
    public ParticleSystem particleSystemToePrefab;

    private bool isPlayerXTurn = true;

    private int crossesWinCount = 0;
    private int toesWinCount = 0;

    public TextMeshProUGUI CrossText;
    public TextMeshProUGUI ToeText;

    void Start()
    {
        InitializeGrid();
    }

    void InitializeGrid()
    {
        grid = new Cell[3, 3];

        GridLayoutGroup gridLayout = GetComponent<GridLayoutGroup>();

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

    void OnCellClick(Cell cell)
    {
        if (cell.IsEmpty)
        {
            Image newSymbol = isPlayerXTurn ? Instantiate(crossPrefab) : Instantiate(toePrefab);
            newSymbol.transform.SetParent(cell.Button.transform, false);

            cell.Button.image.sprite = newSymbol.sprite;

            cell.SetSymbol(newSymbol, isPlayerXTurn);

            grid[cell.Row, cell.Col] = cell;

            cell.Button.image.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            CheckForWinner(cell.Row, cell.Col);

            ParticleSystem particleSystemPrefab = isPlayerXTurn ? particleSystemCrossPrefab : particleSystemToePrefab;

            ActivateParticleEffect(cell.Button.transform.position, particleSystemPrefab);

            isPlayerXTurn = !isPlayerXTurn;
        }
    }

    void ActivateParticleEffect(Vector3 position, ParticleSystem particleSystemPrefab)
    {
        ParticleSystem particleSystem = Instantiate(particleSystemPrefab, position, Quaternion.identity);
        Destroy(particleSystem.gameObject, particleSystem.main.duration);
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

    void CheckForWinner(int lastRow, int lastCol)
    {
        if (grid[lastRow, 0].SymbolType != SymbolType.None && grid[lastRow, 0].SymbolType == grid[lastRow, 1].SymbolType && grid[lastRow, 1].SymbolType == grid[lastRow, 2].SymbolType)
        {
            if (grid[lastRow, 0].SymbolType == SymbolType.Cross)
            {
                Debug.Log("Победа! Крестики выиграли в строке " + (lastRow + 1) + "!");
                crossesWinCount++;
                CrossText.text = crossesWinCount.ToString();
            }
            else if (grid[lastRow, 0].SymbolType == SymbolType.Toe)
            {
                Debug.Log("Победа! Нолики выиграли в строке " + (lastRow + 1) + "!");
                toesWinCount++;
                ToeText.text = toesWinCount.ToString();
            }
        }

        if (grid[0, lastCol].SymbolType != SymbolType.None && grid[0, lastCol].SymbolType == grid[1, lastCol].SymbolType && grid[1, lastCol].SymbolType == grid[2, lastCol].SymbolType)
        {
            if (grid[0, lastCol].SymbolType == SymbolType.Cross)
            {
                Debug.Log("Победа! Крестики выиграли в колонке " + (lastCol + 1) + "!");
                crossesWinCount++;
                CrossText.text = crossesWinCount.ToString();
            }
            else if (grid[0, lastCol].SymbolType == SymbolType.Toe)
            {
                Debug.Log("Победа! Нолики выиграли в колонке " + (lastCol + 1) + "!");
                toesWinCount++;
                ToeText.text = toesWinCount.ToString();
            }
        }

        if ((lastRow == lastCol || lastRow + lastCol == 2) &&
            (grid[0, 0].SymbolType != SymbolType.None && grid[0, 0].SymbolType == grid[1, 1].SymbolType && grid[1, 1].SymbolType == grid[2, 2].SymbolType ||
            grid[0, 2].SymbolType != SymbolType.None && grid[0, 2].SymbolType == grid[1, 1].SymbolType && grid[1, 1].SymbolType == grid[2, 0].SymbolType))
        {
            if (grid[1, 1].SymbolType == SymbolType.Cross)
            {
                Debug.Log("Победа! Крестики выиграли по диагонали!");
                crossesWinCount++;
                CrossText.text = crossesWinCount.ToString();
            }
            else if (grid[1, 1].SymbolType == SymbolType.Toe)
            {
                Debug.Log("Победа! Нолики выиграли по диагонали!");
                toesWinCount++;
                ToeText.text = toesWinCount.ToString();
            }
        }

        Debug.Log("Статистика: Крестики - " + crossesWinCount + ", Нолики - " + toesWinCount);
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
        }

        public void SetButton(Button button)
        {
            Button = button;
        }

        public void SetSymbol(Image symbol, bool isCross)
        {
            Symbol = symbol;
            SymbolType = isCross ? SymbolType.Cross : SymbolType.Toe;
        }
    }
}
