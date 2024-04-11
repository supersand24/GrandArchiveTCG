using Godot;

public partial class Hand : ExtendedZone
{
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

    public void SpawnZones()
    {
        field = GetNode<ExtendedZone>("Field");
        memory = GetNode<ExtendedZone>("Memory");

        GetParent<Game>().silvieDeckImporter.ImportDeck("supersand24", "XXCHvAXEbnGYWJdNkTQI", this);
    }

}