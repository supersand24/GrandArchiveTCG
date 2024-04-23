using Godot;

public partial class Hand : ExtendedZone
{
    [Export] PackedScene stackInstance { get; set; }
    [Export] PackedScene extendedZoneInstance { get; set; }

    [ExportGroup("Zones")]
    [Export] public CardInstance mainDeck;
    [Export] public CardInstance materialDeck;
    [Export] public CardInstance graveyard;
    [Export] public CardInstance banishment;
    [Export] public CardInstance champion;
    [Export] public ExtendedZone field;
    [Export] public ExtendedZone memory;

    public void SpawnZones()
    {
        GetParent<Game>().silvieDeckImporter.ImportDeck("supersand24", "XXCHvAXEbnGYWJdNkTQI", this);
    }

}