using Godot;

[GlobalClass]
public partial class GameZone : Resource
{
    [Export] public string name = "Unknown Zone";
    [Export] bool isPrivate = false;
    [Export] bool isSearchable = true;
    [Export] Ownership ownership = Ownership.Player;

    public enum Ownership
    {
        Shared, Player, Champion
    }

}
