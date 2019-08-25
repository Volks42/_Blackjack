using System.Collections.Generic;
using UnityEngine;

public static class MasterDeck
{
    private static readonly List<SingleCard> matchDeck = new List<SingleCard>();
    private static readonly List<SingleCard> persistentDeck; 

    //constructs the deck, creating 52 card instances and adds to a list 
    static MasterDeck()
    {
        //loading sprite assets
        Dictionary<string, Sprite> cardSprites = new Dictionary<string, Sprite>();
        var loadedCardSprites = Resources.LoadAll("Cards/", typeof(Sprite));
        foreach (Sprite cardSprite in loadedCardSprites)
        {
            cardSprites.Add(cardSprite.name, cardSprite);
        }
        //creating cards with values
        for (int s = 0; s < 4; s++)
        {
            for (int c = 1; c < 14; c++)
            {
                SingleCard card = new SingleCard(c, s);
                cardSprites.TryGetValue(string.Concat("card", card.SuitName, card.ValueName), out var cardSprite);
                card.SetCardSprite(cardSprite);
                matchDeck.Add(card);
            }
        }
        //copys the newly created deck to a persistent deck, to save reloading sprites and values 
        persistentDeck = new List<SingleCard>(matchDeck);
    }

   public static void InitialDeal(PlayerData playerHand, PlayerData dealerHand)
    {
        DrawCard(dealerHand, false, 2);
        DrawCard(playerHand, true, 2);
    }

    //draws a card from the deck and gives it to the player
    public static void DrawCard(PlayerData player, bool isPlayerTurn, int numCards)
    {
        for (int i = 0; i < numCards; i++)
        {
            SingleCard drawnCard = matchDeck[Random.Range(0, matchDeck.Count)];
            matchDeck.Remove(drawnCard);
            player.AddCard(drawnCard, isPlayerTurn);
        }
    }

    public static void DebugDrawForceCard(PlayerData player, bool isPlayerTurn, int cardVal)
    {
        SingleCard drawnCard = new SingleCard(cardVal, 1);
        drawnCard.SetCardSprite(persistentDeck[cardVal - 1].CardSprite);
        player.AddCard(drawnCard, isPlayerTurn);
    }

    public static void ResetMasterDeck()
    {
        matchDeck.Clear();
        matchDeck.AddRange(persistentDeck);
    }
}
