using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public GameObject CardPrefab;
    public GameObject PlayerMatt;
    public GameObject DealerMatt;

    private Button hitButton, standButton;
    private Text playerScoreUI, dealerScoreUI;
    private GameObject gameOverUI;

    private PlayerData playerHand;
    private PlayerData dealerHand;

    public static bool AnimPlaying = false;

    private void Awake()
    {
        hitButton = GameObject.FindGameObjectWithTag("HitButton").GetComponent<Button>();
        standButton = GameObject.FindGameObjectWithTag("StandButton").GetComponent<Button>();
        playerScoreUI = GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<Text>();
        dealerScoreUI = GameObject.FindGameObjectWithTag("DealerUI").GetComponent<Text>();
        gameOverUI = GameObject.FindGameObjectWithTag("GameOver");
    }

    private void Start()
    {
        gameOverUI.SetActive(false);
        dealerHand = new PlayerData(CardPrefab, DealerMatt, dealerScoreUI);
        playerHand = new PlayerData(CardPrefab, PlayerMatt, playerScoreUI);
        MasterDeck.InitialDeal(playerHand, dealerHand);
    }

    public void DrawCardClick()
    {
        MasterDeck.DrawCard(playerHand, true, 1);

        if (playerHand.HandValues[1] == 21 || playerHand.HandValues[0] == 21)
        {
            StandClick();
        }
        else if (playerHand.HandValues[0] > 21)
        {
            StartCoroutine(GameOverText(0));
        }
    }

    public void StandClick()
    {
        hitButton.interactable = false;
        standButton.interactable = false;
        StartCoroutine(DealerDraw());
    }

    public IEnumerator DealerDraw()
    {
        //checks if dealer has blackjack 
        if (dealerHand.HandValues[1] == 21)
        {
            CheckScore(playerHand.HandValues, dealerHand.HandValues);
        }
        else
            while (dealerHand.HandValues[0] <= 17 && !(dealerHand.HandValues[1] > 17 && dealerHand.HandValues[1] < 21))
            {
                MasterDeck.DrawCard(dealerHand, false, 1);
                while (AnimPlaying)
                {
                    yield return null;
                }
            }
        CheckScore(playerHand.HandValues, dealerHand.HandValues);
    }

    public void CheckScore(int[] playerScores, int[] dealerScores)
    {
        int playersHighScore = (playerScores[1] <= 21) ? playerScores[1] : playerScores[0];
        playersHighScore = playersHighScore <= 21 ? playersHighScore : 0;

        int dealersHighScore = (dealerScores[1] <= 21) ? dealerScores[1] : dealerScores[0];
        dealersHighScore = dealersHighScore <= 21 ? dealersHighScore : 0;

        //draw
        if (playersHighScore == dealersHighScore)
        {
            StartCoroutine(GameOverText(2));
        }

        //win 
        else if (playersHighScore > dealersHighScore)
        {
            StartCoroutine(GameOverText(1));
        }

        //lose
        else if (playersHighScore < dealersHighScore)
        {
            StartCoroutine(GameOverText(0));
        }
    }

    public IEnumerator GameOverText(int winState)
    {
        hitButton.interactable = false;
        standButton.interactable = false;
        while (AnimPlaying)
        {
            yield return null;
        }
        gameOverUI.SetActive(true);
        Text[] UItext = gameOverUI.GetComponentsInChildren<Text>();
        switch (winState)
        {
            case (1):
                {
                    UItext[0].text = "You Win! Play Again?";
                }
                break;
            case (0):
                {
                    UItext[0].text = "You Lose. Play Again?";
                }
                break;
            case (2):
                {
                    UItext[0].text = "Draw. Play Again?";
                }
                break;
        }
    }


    public void ResetGame()
    {
        //wipe existing data 
        MasterDeck.ResetMasterDeck();
        playerHand.ResetPlayerData();
        dealerHand.ResetPlayerData();
        gameOverUI.SetActive(false);
        //set up new game 
        MasterDeck.InitialDeal(playerHand, dealerHand);
        //enable buttons 
        hitButton.interactable = true;
        standButton.interactable = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        //keyboard control scheme
        if (Input.GetKeyDown("space") && hitButton.IsInteractable())
        {
            DrawCardClick();
        }

        else if (Input.GetKeyDown("space") && gameOverUI.activeInHierarchy)
        {
            ResetGame();
        }

        if (Input.GetKeyDown("s") && standButton.IsInteractable())
        {
            StandClick();
        }
    }
}
