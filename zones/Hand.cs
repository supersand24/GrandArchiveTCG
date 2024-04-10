using Godot;
using Godot.Collections;

public partial class Hand : ExtendedZone
{
    [Export] PackedScene cardInstance { get; set; }

    [Export] PackedScene stackInstance { get; set; }
    [Export] PackedScene extendedZoneInstance { get; set; }
    public Stack mainDeck;
    public Stack materialDeck;
    public Stack graveyard;
    public Stack banishment;
    public Stack champion;
    public ExtendedZone field;
    public ExtendedZone memory;

    [Export] GameZone testZone;

    //TODO Move to Game
    public Stack highlightedStack = null;

    //Temp Used for Placing Decks At Start.
    int deckPlacement = 675;

    public void SpawnZones()
    {
        mainDeck = GetNode<Stack>("Main Deck");
        materialDeck = GetNode<Stack>("Material Deck");
        graveyard = GetNode<Stack>("Graveyard");
        banishment = GetNode<Stack>("Banishment");
        champion = GetNode<Stack>("Champion Stack");
        field = GetNode<ExtendedZone>("Field");
        memory = GetNode<ExtendedZone>("Memory");

        GetParent<Game>().silvieDeckImporter.ImportDeck("supersand24", "XXCHvAXEbnGYWJdNkTQI", this);

    }

    public void UnhighlightStack()
    {
        if (highlightedStack != null)
            highlightedStack.Unhighlight();
        highlightedStack = null;
    }

    public override void _Input(InputEvent @event)
    {
        if (highlightedStack == null) return;
        if (@event.IsActionPressed("draw"))
        {
            CardInstance card = highlightedStack.MoveTopCardToZone(this);
            card.canPickup = true;
            UpdateCardSpacing();
        }
    }
}