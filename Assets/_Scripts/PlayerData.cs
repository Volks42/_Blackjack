using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData
{
    public int[] HandValues = new int[2];

    private readonly GameObject cardPrefab;
    private readonly GameObject playerMatt;
    private readonly Text scoreText;
    private readonly List<GameObject> heldCards = new List<GameObject>();
    private readonly float cardOffset;
    private readonly Vector3 deckPosition;

    public PlayerData(GameObject cardPrefab, GameObject playerMatt, Text scoreText)
    {
        this.cardPrefab = cardPrefab;
        this.playerMatt = playerMatt;
        this.scoreText = scoreText;
        cardOffset = 1.47f;
        deckPosition = new Vector3(-2.96f, 2.645f, 0f);
    }

    //adds the card to the players hand and validates the hand value 
    public void AddCard(SingleCard newCard, bool isPlayerTurn)
    {
        Vector3 cardPos = new Vector3(playerMatt.transform.position.x + (cardOffset * heldCards.Count), playerMatt.transform.position.y, 0f);
        int sortingNum = heldCards.Count == 0 ? 1 : heldCards.Count == 1 ? 0 : heldCards.Count;

        GameObject instantiatedCard = Object.Instantiate(cardPrefab, deckPosition, Quaternion.identity);
        instantiatedCard.GetComponent<SpriteRenderer>().sortingOrder = sortingNum;
        instantiatedCard.GetComponent<AnimationController>().AnimFunc(newCard.CardSprite, isPlayerTurn, cardPos);

        if (newCard.CardValue == 1)
        {
            HandValues[0] += 1;
            HandValues[1] += 11;
        }
        else
        {
            HandValues[0] += newCard.CardValue;
            HandValues[1] += newCard.CardValue;
        }

        heldCards.Add(instantiatedCard);
        StaticCoroutine.StartCoroutine(UpdateScoreText());
    }

    private IEnumerator UpdateScoreText()
    {
        while (GameLogic.AnimPlaying)
        {
            yield return null;
        }
        scoreText.text = (HandValues[1] <= 21 && HandValues[0] != HandValues[1]) ? (HandValues[0].ToString() + " / " + HandValues[1].ToString()) : HandValues[0].ToString();
    }

    public void ResetPlayerData()
    {
        scoreText.text = "0";
        HandValues[0] = 0;
        HandValues[1] = 0;

        //destroy the card objects so they don't remain in world on reset
        for (int i = 0; i < heldCards.Count; i++)
        {
            Object.Destroy(heldCards[i]);
        }
        heldCards.Clear();
    }
}

