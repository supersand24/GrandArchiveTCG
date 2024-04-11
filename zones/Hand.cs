using Godot;
using Godot.Collections;

public partial class Hand : ExtendedZone
{
    [Export] PackedScene cardInstance { get; set; }

    [Export] PackedScene stackInstance { get; set; }
    [Export] PackedScene extendedZoneInstance { get; set; }

    [ExportGroup("Zones")]
    [Export] public CardStack mainDeck;
    [Export] public CardStack materialDeck;
    [Export] public CardStack graveyard;
    [Export] public CardStack banishment;
    [Export] public CardStack champion;
    public ExtendedZone field;
    public ExtendedZone memory;

    //TODO Move to Game
    public Stack highlightedStack = null;

    //Temp Used for Placing Decks At Start.
    int deckPlacement = 675;

    public void SpawnZones()
    {
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