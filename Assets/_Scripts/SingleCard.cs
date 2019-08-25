using UnityEngine;

public enum CardValues { A = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, J, Q, K };
public enum CardSuits { Clubs, Diamonds, Hearts, Spades };

public class SingleCard
{
    public readonly CardValues ValueName;
    public readonly int CardValue;
    public readonly CardSuits SuitName;
    public Sprite CardSprite { get; private set; }

    //constructs the card and sets it name, suit and value 
    public SingleCard(int cardValueNum, int suitEnumName)
    {
        ValueName = (CardValues)cardValueNum;
        CardValue = cardValueNum < 10 ? cardValueNum : 10;
        SuitName = (CardSuits)suitEnumName;
    }

    public void SetCardSprite(Sprite cardSprite)
    {
        if (cardSprite != null)
        {
            this.CardSprite = cardSprite;
        }
    }
}
