using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private List<Block> BlockList = new List<Block>();
    [SerializeField] private List<int> choiceList = new List<int>();
    [SerializeField] private Sprite circle, cross;
    [SerializeField] private GameOverController gameOverController;
    [SerializeField] private TextMeshProUGUI turnText;
    public bool aiTurn;
    public PlayerTurn playerTurn;
    public GameMode gameMode;
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameMode = (PlayerPrefs.GetInt("GameMode") == 0) ? GameMode.Local : GameMode.AI;
        setTurn();
    }

    void AITurn()
    {
        int bestMove = FindBestMove(choiceList);
        BlockList[bestMove].SetSign();
    }

    public Sprite GetSprite()
    {
        return (playerTurn == PlayerTurn.Player1) ? circle : cross;
    }

    public void SetSign(int choiceListIndex)
    {
        choiceList[choiceListIndex] = (playerTurn == PlayerTurn.Player1) ? 0 : 1;

        if (GetWinnerName()) return;
        if (CheckGameDraw()) return;
        ChangeTurn();
    }

    void setTurn()
    {
        playerTurn = (UnityEngine.Random.Range(0, 2) == 0) ? PlayerTurn.Player1 : PlayerTurn.Player2;
        RefreshPlayerTurnText();

        if (aiTurn)
        {
            Invoke(nameof(AITurn), .3f);
        }
    }

    void ChangeTurn()
    {
        playerTurn = (playerTurn == PlayerTurn.Player1) ? PlayerTurn.Player2 : PlayerTurn.Player1;
        RefreshPlayerTurnText();

        if (aiTurn)
        {
            Invoke(nameof(AITurn), .3f);
        }
    }

    // Winning combinations (row-major order)
    private int[][] winningCombinations = new int[][]
    {
        new int[] { 0, 1, 2 }, // Row 1
        new int[] { 3, 4, 5 }, // Row 2
        new int[] { 6, 7, 8 }, // Row 3
        new int[] { 0, 3, 6 }, // Column 1
        new int[] { 1, 4, 7 }, // Column 2
        new int[] { 2, 5, 8 }, // Column 3
        new int[] { 0, 4, 8 }, // Diagonal 1
        new int[] { 2, 4, 6 }  // Diagonal 2
    };

    // Method to check if there's a winner
    public int CheckWinner()
    {
        foreach (var combo in winningCombinations)
        {
            // Check if all three indices in a combination have the same non-zero value
            if (choiceList[combo[0]] != -1 &&
                choiceList[combo[0]] == choiceList[combo[1]] &&
                choiceList[combo[1]] == choiceList[combo[2]])
            {
                return choiceList[combo[0]]; // Return the winner (1 or 2)
            }
        }

        // No winner
        return -1;
    }

    public bool CheckGameDraw()
    {
        foreach (var choice in choiceList)
        {
            if (choice == -1)
            {
                return false;
            }
        }

        gameOverController.GameOver();
        return true;
    }

    bool GetWinnerName()
    {
        if (CheckWinner() != -1)
        {
            string winner = (CheckWinner() == 0) ? PlayerTurn.Player1.ToString() : PlayerTurn.Player2.ToString();

            if (gameMode == GameMode.AI)
            {
                winner = (CheckWinner() == 0) ? "You" : "AI";
            }

            gameOverController.GameOver(winner);
            return true;
        }

        return false;
    }


    // Minimax algorithm
    private int Minimax(List<int> board, int depth, bool isMaximizing)
    {
        int winner = CheckWinner();
        if (winner == 0) return -10; // Player wins
        if (winner == 1) return 10;  // AI wins
        if (!board.Contains(-1)) return -1; // Draw

        if (isMaximizing)
        {
            int maxEval = int.MinValue;
            for (int i = 0; i < board.Count; i++)
            {
                if (board[i] == -1)
                {
                    board[i] = 1; // AI's move
                    int eval = Minimax(board, depth + 1, false);
                    board[i] = -1; // Undo move
                    maxEval = Math.Max(maxEval, eval);
                }
            }
            return maxEval;
        }
        else
        {
            int minEval = int.MaxValue;
            for (int i = 0; i < board.Count; i++)
            {
                if (board[i] == -1)
                {
                    board[i] = 0; // Player's move
                    int eval = Minimax(board, depth + 1, true);
                    board[i] = -1; // Undo move
                    minEval = Math.Min(minEval, eval);
                }
            }
            return minEval;
        }
    }
    // Find the best move for the AI
    private int FindBestMove(List<int> board)
    {
        int bestMove = -1;
        int bestValue = int.MinValue;

        for (int i = 0; i < board.Count; i++)
        {
            if (board[i] == -1)
            {
                board[i] = 1; // AI's move
                int moveValue = Minimax(board, 0, false);
                board[i] = -1; // Undo move

                if (moveValue > bestValue)
                {
                    bestValue = moveValue;
                    bestMove = i;
                }
            }
        }

        return bestMove;
    }

    void RefreshPlayerTurnText()
    {
        if (gameMode == GameMode.Local)
        {
            turnText.text = playerTurn + " Turn";
        }
        else
        {
            string turn = (playerTurn == PlayerTurn.Player1) ? "Your" : "AI";
            turnText.text = turn + " Turn";

            aiTurn = (playerTurn == PlayerTurn.Player2);
        }
    }
}

public enum PlayerTurn { Player1, Player2 }
public enum GameMode { Local, AI }
