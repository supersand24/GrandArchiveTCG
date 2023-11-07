using Godot;

public partial class Zone : Node2D
{
    [Export(PropertyHint.Enum, "Lower,Higher")] public int layer = 0;

    [Export] public bool privateZone = false;
    [Export(PropertyHint.Range, "0,1,")] public int owner = 0;

}
