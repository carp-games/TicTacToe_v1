using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TicTacToeController : MonoBehaviour {

    [Tooltip("Player 1 is played by the AI")]
    [HideInInspector]
    public bool p1Ai = false;

    [Tooltip("Player 2 is played by the AI")]
    [HideInInspector]
    public bool p2Ai = true;

    [Tooltip("Using hard coded instructions for the first two moves to improve the speed")]
    [HideInInspector]
    public bool useShortcuts = true;

    [Tooltip("Visualize the AI algorithm step by step")]
    [HideInInspector]
    public bool visualizeAI = false;

    [Tooltip("Duration of each AI algorithm step in seconds")]
    [HideInInspector]
    public float algorithmStepDuration = 1;

    [SerializeField]
    private List<Button> buttons = new List<Button>();

    public delegate void OnGameOverDelegate(int win);
    public OnGameOverDelegate onGameOverDelegate;

    private bool turn; // true: Player, false: AI
    private int fieldsLeft;
    private bool isGameOver = true;

    // Will hold the current values of the MinMax algorithm
    private int recursionScore;
    private int optimalScoreButtonIndex = -1;

    public void StartGame() {
        turn = Mathf.Round(UnityEngine.Random.Range(0, 1)) == 1;
        Reset();
    }

    private void EnableButtons(bool enabled, bool ignoreEmpty = false) {
        foreach (Button button in buttons) {
            // Do not reanable buttons that already were used
            if (!enabled || ignoreEmpty || IsFieldEmpty(button)) {
                button.interactable = enabled;
            }
        }
    }

    private bool IsFieldEmpty(Button button) {
        return GetText(button).text == "";
    }

    private Text GetText(Button button) {
        return button.GetComponentInChildren<Text>();
    }

    private bool SetMarkAndCheckForWin(Button button, bool colorate = false) {
        Text text = GetText(button);
        if (text.text != "") {
            return false;
        }
        text.text = turn ? "X" : "O";
        fieldsLeft--;

        return CheckForWin(text.text, colorate);
    }

    public void OnButtonClick(Button button) {
        if (isGameOver) {
            Reset();
            return;
        }
        if (fieldsLeft <= 0) {
            return;
        }

        if (SetMarkAndCheckForWin(button, true)) {
            Win(); // Display the game results
        }
        button.interactable = false;

        // Game Over - Draw
        if (fieldsLeft <= 0) {
            GameOverDraw();
        }

        // Switch turns
        turn = !turn;

        // Let the AI play
        if (!isGameOver && fieldsLeft > 0 && IsAiTurn()) {
            StartCoroutine(AiTurnCoroutine());
        }
    }

    private bool IsAiTurn() {
        return (turn && p1Ai) || (!turn && p2Ai);
    }

    private IEnumerator AiTurnCoroutine() {
        EnableButtons(false);
        // Call the MinMax algorithm. It will store the (for the player) worst move in optimalScoreButtonIndex.
        // What is worst for the player, is the best for the AI.
        IEnumerator minMaxEnumerator = MinMaxCoroutine(1);
        if (visualizeAI) {
            // Take breaks between all steps so we can see it
            yield return StartCoroutine(minMaxEnumerator);
        } else {
            // Force the coroutine to do everything in one frame
            while (minMaxEnumerator.MoveNext()) {}
        }

        HideDepthAndScoreForAllButtons();

        // Debug.Log("buttonIndex: " + optimalScoreButtonIndex);
        Button button = buttons[optimalScoreButtonIndex]; // Could be random by using: (int)Mathf.Round(Random.Range(0, 9));
        EnableButtons(true);
        OnButtonClick(button);
    }

    /// <summary>
    /// Min Max algorithm to find the best and worse moves.
    /// This Method stores the current best and worst moves in
    /// highestCurrentScoreIndex and lowestCurrentScoreIndex as a side effect.
    /// </summary>
    /// <param name="depth">Depth - the number of recursion step for weighting the scores</param>
    /// <returns>The sum of scores of all possible steps from the current recursion level downwards (stored in recursionScore)</returns>
    private IEnumerator MinMaxCoroutine(int depth) {
        // Base case and shortcuts (hard coded moves) to stop recursion
        if (CheckBaseCaseAndShortcuts()) {
            yield break;
        }

        // We want to store which field gives us the best (player) or the worst (CPU) score
        int currentBestScore = turn ? Int32.MinValue : Int32.MaxValue;
        int currentOptimalScoreButtonIndex = -1;

        // Find next free field
        int fieldIndex = 0;
        while (fieldIndex < buttons.Count) {
            if (IsFieldFree(fieldIndex)) {
                Button button = buttons[fieldIndex];
                int currentScore = 0;

                bool endRecursion = false;

                // Some delay to make it possible to see single steps
                if (visualizeAI && algorithmStepDuration > 0) {
                    yield return new WaitForSeconds(algorithmStepDuration);
                }
                SetDepth(button, depth);

                // End iteration and recursion level when we win, because we don't need to go deeper
                if (SetMarkAndCheckForWin(button)) {
                    // Debug.Log("Found a winner: " + GetText(button).text);
                    currentScore = (turn ? 1 : -1) * (10 - depth);
                    endRecursion = true;
                } else if (fieldsLeft > 0) {
                    // If there are fields left after the SetMarkAndCheckForWin we can go deeper in the recursion
                    turn = !turn; // Switch turns - in the next step we want to simulate the other player

                    IEnumerator minMaxEnumerator = MinMaxCoroutine(depth + 1);
                    if (visualizeAI) {
                        // Take breaks between all steps so we can see it
                        yield return StartCoroutine(minMaxEnumerator);
                    } else {
                        // Force the coroutine to do everything in one frame
                        while (minMaxEnumerator.MoveNext()) { }
                    }
                    currentScore = recursionScore;
                    turn = !turn; // Switch turns back
                }

                if ((turn && currentScore > currentBestScore) || (!turn && currentScore < currentBestScore)) {
                    currentBestScore = currentScore;
                    currentOptimalScoreButtonIndex = fieldIndex;
                }

                if (visualizeAI) {
                    SetScore(button, currentScore);
                }

                // Some delay to make it possible to see single steps
                if (visualizeAI && algorithmStepDuration > 0) {
                    yield return new WaitForSeconds(algorithmStepDuration);
                }

                // Undo this step and go to the next field
                GetText(button).text = "";
                if (visualizeAI) {
                    HideDepthAndScore(button);
                }
                fieldsLeft++;

                if (endRecursion) {
                    // No need to check further fields if there already is a win
                    break;
                }
            }
            fieldIndex++;
            // Stop if we checked all buttons
        }

        recursionScore = currentBestScore;
        optimalScoreButtonIndex = currentOptimalScoreButtonIndex;
        // Debug.Log("score: " + recursionScore);
    }

    private void SetScore(Button button, int score) {
        Text scoreText = button.transform.GetChild(2).GetComponent<Text>();
        scoreText.gameObject.SetActive(true);
        scoreText.text = score + " Points";
    }

    private void SetDepth(Button button, int depth) {
        Text depthText = button.transform.GetChild(1).GetComponent<Text>();
        depthText.gameObject.SetActive(true);
        depthText.text = "Depth: " + depth;

        GetText(button).color = Color.gray;
    }

    private void HideDepthAndScore(Button button) {
        button.transform.GetChild(1).gameObject.SetActive(false);
        button.transform.GetChild(2).gameObject.SetActive(false);
        GetText(button).color = Color.white;
    }

    private void HideDepthAndScoreForAllButtons() {
        foreach (Button button in buttons) {
            HideDepthAndScore(button);
        }
    }

    // This will return true if we can stop the recursion immediately and handle shortcuts (hard coded moves for faster progress)
    private bool CheckBaseCaseAndShortcuts() {
        if (fieldsLeft <= 0) {
            recursionScore = 0;
            return true;
        }

        if (!useShortcuts) {
            return false;
        }

        // No need to calculate anything if all fields are free - any corner is the best.
        // But let's use that chance for some variety and use random. Index will be 0, 2, 6 or 8
        if (fieldsLeft == 9) {
            RandomCorner();
            return true;
        }
        // Shortcut for the optimal second move after an opening
        if (fieldsLeft == 8) {
            // If the other player used the middle. go for any corner
            if (!GetText(buttons[4]).text.Equals("")) {
                RandomCorner();
            } else { // Else the middle is always the best
                optimalScoreButtonIndex = 4;
            }
            return true;
        }
        return false;
    }

    // Returns true if the given mark if present with three in a row
    private bool CheckForWin(string mark, bool colorate = false) {
        if (fieldsLeft > 6) {
            return false;
        }
        // Horizontal
        if (CompareButtons(0, 1, 2, mark, colorate)
         || CompareButtons(3, 4, 5, mark, colorate)
         || CompareButtons(6, 7, 8, mark, colorate)
        // Vertical
         || CompareButtons(0, 3, 6, mark, colorate)
         || CompareButtons(1, 4, 7, mark, colorate)
         || CompareButtons(2, 5, 8, mark, colorate)
        // Diagonal
         || CompareButtons(0, 4, 8, mark, colorate)
         || CompareButtons(6, 4, 2, mark, colorate)) {
            return true;
        }
        return false;
    }

    private bool CompareButtons(int ind1, int ind2, int ind3, string mark, bool colorate = false) {
        Text text1 = GetText(buttons[ind1]);
        Text text2 = GetText(buttons[ind2]);
        Text text3 = GetText(buttons[ind3]);
        bool equal = text1.text == mark
                  && text2.text == mark
                  && text3.text == mark;
        if (colorate && equal) {
            Color color = turn ? Color.green : Color.red;
            text1.color = color;
            text2.color = color;
            text3.color = color;
        }
        return equal;
    }

    // Checks if a field is still free
    private bool IsFieldFree(int index) => GetText(buttons[index]).text.Length == 0;

    // Displays the game results
    private void Win() {
        // Debug.Log(turn ? "Player 1 won!" : "Game Over!");
        isGameOver = true;
        EnableButtons(false);
        onGameOverDelegate?.Invoke(turn ? 0 : 1);
    }
    private void GameOverDraw() {
        // Debug.Log("Game Over - Draw");
        isGameOver = true;
        EnableButtons(false);
        onGameOverDelegate?.Invoke(-1);
    }

    // Use some variety and use random to determine an optimal start field. Index will be 0, 2, 6 or 8
    private int RandomCorner() {
        optimalScoreButtonIndex = (int)Mathf.Floor(UnityEngine.Random.Range(0, 4));
        if (optimalScoreButtonIndex == 1) {
            optimalScoreButtonIndex = 6;
        } else if (optimalScoreButtonIndex == 3) {
            optimalScoreButtonIndex = 8;
        }
        return optimalScoreButtonIndex;
    }


    // Right click on the script in the inspector to use these methods

    [ContextMenu("Reset")]
    private void Reset() {
        foreach (Button button in buttons) {
            Text text = GetText(button);
            text.color = Color.white;
            text.text = "";
            button.interactable = true;
        }
        fieldsLeft = 9;
        isGameOver = false;
        if (IsAiTurn()) {
            StartCoroutine(AiTurnCoroutine());
        }
    }

    // Right click on the script in the inspector to use this
    [ContextMenu("Set Depth Test Example")]
    private void SetDepthTestExample() {
        turn = true;
        Reset();
        GetText(buttons[1]).text = "X"; buttons[1].interactable = false;
        GetText(buttons[5]).text = "X"; buttons[5].interactable = false;
        GetText(buttons[6]).text = "O"; buttons[6].interactable = false;
        GetText(buttons[7]).text = "O"; buttons[7].interactable = false;
        GetText(buttons[8]).text = "X"; buttons[8].interactable = false;

        turn = false;
        fieldsLeft = 4;
        StartCoroutine(AiTurnCoroutine());
    }
}
