public partial class Card
{
    CardEditionData card;
    bool faceUp;

    public Card(CardEditionData card, bool faceUp = true)
    {
        this.card = card;
        this.faceUp = faceUp;
    }

    public CardEditionData GetData()
    {
        return card;
    }
}
