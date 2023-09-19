using Godot;
using Godot.Collections;
using System.Text;

public partial class CardData
{
    public string uuid { get; }
    public string name { get; }
    public string slug { get; }

    public Array<string> types { get; }
    public Array<string> subtypes { get; }
    public Array<string> classes { get; }
    public string element { get; }

    public string effect;
    public string effectFormatted;
    public string flavor;

    public Variant costReserve { get; }
    public Variant costMemory { get; }
    public Variant level { get; }

    public Variant power { get; }
    public Variant life { get; }
    public Variant durability { get; }
    public Variant speed { get; }

    public System.Collections.Generic.List<CardEditionData> editions = new();

    public CardData(Dictionary data)
    {
        Variant temp;

        data.TryGetValue("uuid", out temp);
        uuid = temp.As<string>();
        data.TryGetValue("name", out temp);
        name = temp.As<string>();
        data.TryGetValue("slug", out temp);
        slug = temp.As<string>();

        data.TryGetValue("types", out temp);
        types = temp.As<Array<string>>();
        data.TryGetValue("subtypes", out temp);
        subtypes = temp.As<Array<string>>();
        data.TryGetValue("classes", out temp);
        classes = temp.As<Array<string>>();

        data.TryGetValue("effect", out temp);
        effect = temp.As<string>();
        data.TryGetValue("effect_raw", out temp);
        effectFormatted = temp.As<string>();
        data.TryGetValue("flavor", out temp);
        flavor = temp.As<string>();

        data.TryGetValue("cost_memory", out temp);
        costMemory = temp;
        data.TryGetValue("cost_reserve", out temp);
        costReserve = temp;
        data.TryGetValue("level", out temp);
        level = temp;

        data.TryGetValue("power", out temp);
        power = temp;
        data.TryGetValue("life", out temp);
        life = temp;
        data.TryGetValue("durability", out temp);
        durability = temp;
        data.TryGetValue("speed", out temp);
        speed = temp;

        data.TryGetValue("editions", out temp);
        foreach (Dictionary entry in temp.AsGodotArray())
        {
            editions.Add(new CardEditionData(entry));
        }

        GD.Print(ToString());
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("UUID : ").Append(uuid).Append("\n");
        sb.Append("Name : ").Append(name).Append("\n");
        sb.Append("Slug : ").Append(slug).Append("\n");

        sb.Append("Types : ").Append(types).Append("\n");
        sb.Append("Subtypes : ").Append(subtypes).Append("\n");
        sb.Append("Classes : ").Append(classes).Append("\n");

        if (costReserve.VariantType != Variant.Type.Nil) sb.Append("Reserve Cost : ").Append(costReserve).Append("\n");
        if (costMemory.VariantType != Variant.Type.Nil) sb.Append("Memory Cost : ").Append(costMemory).Append("\n");
        if (level.VariantType != Variant.Type.Nil) sb.Append("Level : ").Append(level).Append("\n");

        if (power.VariantType != Variant.Type.Nil) sb.Append("Power : ").Append(power).Append("\n");
        if (life.VariantType != Variant.Type.Nil) sb.Append("Life : ").Append(life).Append("\n");
        if (durability.VariantType != Variant.Type.Nil) sb.Append("Durability : ").Append(durability).Append("\n");
        if (speed.VariantType != Variant.Type.Nil) sb.Append("Speed : ").Append(speed).Append("\n");

        return sb.ToString();
    }
}
