using Godot;
using Godot.Collections;

public partial class Hand : Node2D
{
    [Export] PackedScene cardInstance { get; set; }
    public Array<CardInstance> cards = new();

    [Export] PackedScene stackInstance { get; set; }
    public Stack mainDeck;
    public Stack materialDeck;
    public Stack graveyard;
    public Stack banishment;
    public Stack champion;

    [Export] PackedScene zoneInstance { get; set; }
    public Zone field;

    public Stack highlightedStack = null;

    int handBounds = 400;
    int deckPlacement = 675;

    public void SpawnZones()
    {
        mainDeck = stackInstance.Instantiate<Stack>();
        AddChild(mainDeck);
        mainDeck.Name = "Main Deck";
        mainDeck.Position = new Vector2(deckPlacement, -300);

        graveyard = stackInstance.Instantiate<Stack>();
        AddChild(graveyard);
        graveyard.Name = "Graveyard";
        graveyard.Position = new Vector2(deckPlacement, -100);
        graveyard.privateZone = false;

        champion = stackInstance.Instantiate<Stack>();
        AddChild(champion);
        champion.Name = "Champion Stack";
        champion.Position = new Vector2(-deckPlacement, -500);
        champion.privateZone = false;

        materialDeck = stackInstance.Instantiate<Stack>();
        AddChild(materialDeck);
        materialDeck.Name = "Material Deck";
        materialDeck.Position = new Vector2(-deckPlacement, -300);

        banishment = stackInstance.Instantiate<Stack>();
        AddChild(banishment);
        banishment.Name = "Banishment";
        banishment.Position = new Vector2(-deckPlacement, -100);
        banishment.privateZone = false;

        field = zoneInstance.Instantiate<Zone>();
        AddChild(field);
        field.Name = "Field";
        field.Position = Vector2.Up * 300;

        GetParent<Game>().silvieDeckImporter.ImportDeck("supersand24", "XXCHvAXEbnGYWJdNkTQI", this);

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

    public void UnhighlightStack()
    {
        if (highlightedStack != null)
            highlightedStack.Unhighlight();
        highlightedStack = null;
    }

    public void UpdateHandSpacing()
    {
        switch (cards.Count)
        {
            case 0:
                break;
            case 1:
                cards[0].posGoal = GlobalPosition;
                break;
            default:
                float step = handBounds * 2 / (cards.Count - 1);
                Vector2 newPos = GlobalPosition;
                newPos.X -= handBounds;
                foreach (CardInstance card in cards)
                {
                    card.posGoal = newPos;
                    newPos.X += step;
                }
                break;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (highlightedStack == null) return;
        if (@event.IsActionPressed("draw"))
        {
            CardInstance card = highlightedStack.DrawCard();
            if (card != null) cards.Add(card);
            UpdateHandSpacing();
        }
    }
}