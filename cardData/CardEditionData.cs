using Godot;
using Godot.Collections;
using System.Text;

public partial class CardEditionData
{
    public string uuidEdition;
    public string uuidBase;
    public string slug;

    public CardEditionData(Dictionary data)
    {
        Variant temp;

        data.TryGetValue("uuid", out temp);
        uuidEdition = temp.As<string>();

        data.TryGetValue("card_id", out temp);
        uuidBase = temp.As<string>();

        data.TryGetValue("slug", out temp);
        slug = temp.As<string>();

        GD.Print(slug);

    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("UUID Edition : ").Append(uuidEdition).Append("\n");
        sb.Append("UUID Base : ").Append(uuidBase).Append("\n");
        sb.Append("Slug : ").Append(slug).Append("\n");
        return sb.ToString();
    }
}
