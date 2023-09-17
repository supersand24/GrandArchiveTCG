using Godot;
using Godot.Collections;

public partial class Hand : Node2D
{
    [Export] PackedScene cardInstance { get; set; }
    public Array<CardInstance> cards = new();

    [Export] PackedScene stackInstance { get; set; }
    [Export] Stack mainDeck;
    [Export] Stack materialDeck;
    [Export] Stack graveyard;
    [Export] Stack banishment;

    [Export] int bounds = 400;
    int deckPlacement = 675;

    public void SpawnZones()
    {
        mainDeck = stackInstance.Instantiate<Stack>();
        AddChild(mainDeck);
        mainDeck.Name = "Main Deck";
        mainDeck.Position = new Vector2(deckPlacement, -100);

        materialDeck = stackInstance.Instantiate<Stack>();
        AddChild(materialDeck);
        materialDeck.Position = new Vector2(-deckPlacement, -100);

        graveyard = stackInstance.Instantiate<Stack>();
        AddChild(graveyard);
        graveyard.Position = new Vector2(deckPlacement, -300);

        banishment = stackInstance.Instantiate<Stack>();
        AddChild(banishment);
        banishment.Position = new Vector2(-deckPlacement, -300);

        DrawHand();

    }

    public void DrawHand()
    {
        for (int i = 0; i < 5; i++)
        {
            CardInstance drawnCard = mainDeck.DrawCard();
            if (drawnCard != null)
                cards.Add(drawnCard);
        }
        UpdateHandSpacing();
    }

    public void UpdateHandSpacing()
    {
        //Array<Node> cards = GetChildren();
        switch (cards.Count)
        {
            case 0:
                break;
            case 1:
                cards[0].posGoal = GlobalPosition;
                break;
            default:
                float step = bounds * 2 / (cards.Count - 1);
                Vector2 newPos = GlobalPosition;
                newPos.X -= bounds;
                foreach (CardInstance card in cards)
                {
                    card.posGoal = newPos;
                    newPos.X += step;
                }
                break;
        }

    }
}