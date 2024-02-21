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

    ////Win Panel
    public GameObject WinPanel;
    public TMP_Text resultText; 

    ////Start Screen Selection
    public GameObject chooseMarkerPanel;

    private void Start()
    {
        resultText.text = "Player: " + "\n" + "Virus: " ;
        chooseMarkerPanel.SetActive(true);
    }

    public void ChoosePlayerX()
    {
        playerSprite = XSprite;
        npcSprite = OSprite;

        Debug.Log("player selected X");

        chooseMarkerPanel.SetActive(false);
        EnableButtons();
        
    }

    public void ChoosePlayerO()
    {
        playerSprite = OSprite;
        npcSprite = XSprite;

        Debug.Log("player selected O");

        chooseMarkerPanel.SetActive(false);
        EnableButtons();
        
    }

    void EnableButtons()
    {
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }

    public void OnButtonClick(Button button)
    {
        if (button.interactable && playerTurn)
        {
            button.interactable = false;
            button.image.sprite = playerSprite;
            button.interactable = false;
            playerTurn = false;
            if (CheckForWin(playerSprite))
            {
                DisableAllButtons();
                WinPanel.SetActive(true);
                resultText.text = "Player Wins!"; 
                return;
            }
            else if (CheckForDraw())
            {
                DisableAllButtons();
                WinPanel.SetActive(true);
                resultText.text = "It's a Draw!";
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
            DisableAllButtons();
            WinPanel.SetActive(true);
            resultText.text = "Virus Wins!";
            return;
        }
        else if (CheckForDraw())
        {
            DisableAllButtons();
            WinPanel.SetActive(true);
            resultText.text = "It's a Draw!";
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
                return false; // If any button is still empty or interactable, the game is not a draw
            }
        }
        return true; // If all buttons are filled and there is no winner, it's a draw
    }

    
    void DisableAllButtons()
    {
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
    }

    public void RestartGame()
    {
        foreach (Button button in buttons)
        {
            button.image.sprite = defaultSprite;
            button.interactable = false;
        }

        playerTurn = true;
        WinPanel.SetActive(false);
        chooseMarkerPanel.SetActive(true);
    }

}
