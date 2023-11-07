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

    //TODO Move to Game
    public Stack highlightedStack = null;

    //Temp Used for Placing Decks At Start.
    int deckPlacement = 675;

    public void SpawnZones()
    {
        mainDeck = stackInstance.Instantiate<Stack>();
        AddChild(mainDeck);
        mainDeck.Name = "Main Deck";
        mainDeck.privateZone = true;
        mainDeck.layer = 0;
        mainDeck.Position = new Vector2(deckPlacement, -300);

        graveyard = stackInstance.Instantiate<Stack>();
        AddChild(graveyard);
        graveyard.Name = "Graveyard";
        graveyard.privateZone = false;
        graveyard.layer = 0;
        graveyard.Position = new Vector2(deckPlacement, -100);
        graveyard.privateZone = false;

        champion = stackInstance.Instantiate<Stack>();
        AddChild(champion);
        champion.Name = "Champion Stack";
        champion.privateZone = false;
        champion.layer = 0;
        champion.Position = new Vector2(-deckPlacement, -500);
        champion.privateZone = false;

        materialDeck = stackInstance.Instantiate<Stack>();
        AddChild(materialDeck);
        materialDeck.Name = "Material Deck";
        materialDeck.privateZone = true;
        materialDeck.layer = 0;
        materialDeck.Position = new Vector2(-deckPlacement, -300);

        banishment = stackInstance.Instantiate<Stack>();
        AddChild(banishment);
        banishment.Name = "Banishment";
        banishment.privateZone = false;
        banishment.layer = 0;
        banishment.Position = new Vector2(-deckPlacement, -100);
        banishment.privateZone = false;

        field = extendedZoneInstance.Instantiate<ExtendedZone>();
        AddChild(field);
        field.Name = "Field";
        field.privateZone = false;
        field.layer = 0;
        field.Position = Vector2.Up * 300;

        memory = extendedZoneInstance.Instantiate<ExtendedZone>();
        AddChild(memory);
        memory.Name = "Memory";
        memory.privateZone = true;
        memory.layer = 0;

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
            highlightedStack.MoveCardToZone(this);
            UpdateCardSpacing();
        }
    }
}