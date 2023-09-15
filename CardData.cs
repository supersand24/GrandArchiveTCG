using Godot;
using Godot.Collections;

[GlobalClass]
public partial class CardData : Resource
{
    [Export] public string uuid;
    [Export] public string name;
    [Export] public string slug;

    public Array<CardEditionData> editions = new();

    public CardData(Dictionary data)
    {
        Variant temp;

        data.TryGetValue("uuid", out temp);
        uuid = temp.As<string>();

        data.TryGetValue("name", out temp);
        name = temp.As<string>();

        data.TryGetValue("slug", out temp);
        slug = temp.As<string>();

        data.TryGetValue("editions", out temp);
        foreach (Dictionary entry in temp.AsGodotArray())
        {
            editions.Add(new CardEditionData(entry));
        }

        GD.Print(name + " | " + uuid);
    }
}
