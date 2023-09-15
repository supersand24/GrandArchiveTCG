using Godot;
using Godot.Collections;

[GlobalClass]
public partial class CardEditionData : Resource
{
    [Export] public string uuid;
    [Export] public string card_id;
    [Export] public string slug;

    public CardEditionData(Dictionary data)
    {
        Variant temp;

        data.TryGetValue("uuid", out temp);
        uuid = temp.As<string>();

        data.TryGetValue("card_id", out temp);
        card_id = temp.As<string>();

        data.TryGetValue("slug", out temp);
        slug = temp.As<string>();

        GD.Print(slug);

    }
}
