using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;


public class TicTacToe_Logic : MonoBehaviour
{
    public Button[] buttons;
    public Sprite XSprite;
    public Sprite OSprite;
    public Sprite defaultSprite;

    private Sprite playerSprite;
    private Sprite npcSprite;

    private bool playerTurn = true;


    //Score tracking 
    public int playerScore;
    public int virusScore;
    public GameObject playerScoreSprite;
    public GameObject virusScoreSprite;
    public Sprite[] scoreSprites;

    //Round tracking
    public int roundsPlayed;
    public GameObject[] roundCountUI;
    public GameObject[] roundResult;

    public GameObject annoyedVirus;

    ////Win Panel
    // public GameObject resultPanel;
    public GameObject goodEnding;
    public GameObject badEnding;

    ////Start Screen Selection
    public GameObject chooseMarkerPanel;

    // --------------------------------------------------------------------------------------------------

    private void Start()
    {
        chooseMarkerPanel.SetActive(true);
        playerScore = 0;
        virusScore = 0;
        roundsPlayed = 0;
        UpdateScoreImages();
    }

    public void ChoosePlayerX()
    {
        playerSprite = XSprite;
        npcSprite = OSprite;
        chooseMarkerPanel.SetActive(false);
        EnableButtons();
        roundCountUI[roundsPlayed].SetActive(true);
        //Debug.Log("player selected X");

    }

    public void ChoosePlayerO()
    {
        playerSprite = OSprite;
        npcSprite = XSprite;
        chooseMarkerPanel.SetActive(false);
        EnableButtons();
        //Debug.Log("player selected O");

    }

    void EnableButtons()
    {
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }

    void DisableAllButtons()
    {
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
    }

    public void OnButtonClick(Button button)
    {
        if (button.interactable && playerTurn)
        {
            annoyedVirus.SetActive(false);
            button.interactable = false;
            button.image.sprite = playerSprite;
            button.interactable = false;
            playerTurn = false;
            if (CheckForWin(playerSprite))
            {
                PlayerWins();
                return;
            }
            else if (CheckForDraw())
            {
                Draw();
                return;
            }

            NPCMove();

        }
    }

    void NPCMove()
    {
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, buttons.Length);
        } while (!buttons[randomIndex].interactable);

        buttons[randomIndex].interactable = false;
        buttons[randomIndex].image.sprite = npcSprite;
        playerTurn = true;

        if (CheckForWin(npcSprite))
        {
            VirusWins();
            return;
        }
        else if (CheckForDraw())
        {
            Draw();
            return;
        }
    }

    bool CheckForWin(Sprite marker)
    {
        // Check rows
        for (int row = 0; row < 3; row++)
        {
            if (buttons[row * 3].image.sprite == marker && buttons[row * 3 + 1].image.sprite == marker && buttons[row * 3 + 2].image.sprite == marker)
            {
                return true;
            }
        }

        // Check columns
        for (int col = 0; col < 3; col++)
        {
            if (buttons[col].image.sprite == marker && buttons[col + 3].image.sprite == marker && buttons[col + 6].image.sprite == marker)
            {
                return true;
            }
        }

        // Check diagonals
        if (buttons[0].image.sprite == marker && buttons[4].image.sprite == marker && buttons[8].image.sprite == marker)
        {
            return true;
        }
        if (buttons[2].image.sprite == marker && buttons[4].image.sprite == marker && buttons[6].image.sprite == marker)
        {
            return true;
        }
        return false; // No win condition met
    }

    bool CheckForDraw()
    {
        foreach (Button button in buttons)
        {
            if (button.image.sprite == null || button.interactable)
            {
                return false;
            }
        }
        return true;
    }


    void PlayerWins()
    {
        playerScore++;
        roundsPlayed++;
        annoyedVirus.SetActive(true);
        UpdateScoreImages();
        DisableAllButtons();
        StartCoroutine(ShowResult(roundResult[0]));
    }

    void VirusWins()
    {
        virusScore++;
        roundsPlayed++;
        UpdateScoreImages();
        DisableAllButtons();
        StartCoroutine(ShowResult(roundResult[1]));
    }

    void Draw()
    {
        roundsPlayed++;
        UpdateScoreImages();
        DisableAllButtons();
        StartCoroutine(ShowResult(roundResult[2]));
    }

    void EndGame()
    {
        UpdateScoreImages();

        if (playerScore >= 2)
        {
            goodEnding.SetActive(true);
            Debug.Log("Good Ending");
        }
        else if (virusScore >= 2)
        {
            badEnding.SetActive(true);
            Debug.Log("Bad Ending");
        }
        else if (roundsPlayed >= 3)
        {
            badEnding.SetActive(true);
            Debug.Log("End of the game without clear winner");
        }
        else
        {
            RestartGame();
        }
    }

    void UpdateScoreImages()
    {
        playerScoreSprite.GetComponent<Image>().sprite = scoreSprites[playerScore];
        virusScoreSprite.GetComponent<Image>().sprite = scoreSprites[virusScore];
    }


    IEnumerator ShowResult(GameObject resultObject)
    {
        Debug.Log("coroutine started ---> shoing result");
        resultObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        resultObject.SetActive(false);


        if (roundsPlayed >= 3)
        {
            EndGame();
        }
        else
        {
            RestartGame();
        }
    }

    public void RestartGame()
    {
        foreach (Button button in buttons)
        {
            button.image.sprite = defaultSprite;
            button.interactable = true;
        }
        playerTurn = true;
        roundCountUI[roundsPlayed].SetActive(true);
    }

}
