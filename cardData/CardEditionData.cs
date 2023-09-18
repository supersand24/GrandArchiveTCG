using Godot;
using Godot.Collections;
using System.Text;

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

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("UUID : ").Append(uuid).Append("\n");
        sb.Append("CardID : ").Append(card_id).Append("\n");
        sb.Append("Slug : ").Append(slug).Append("\n");
        return sb.ToString();
    }
}
