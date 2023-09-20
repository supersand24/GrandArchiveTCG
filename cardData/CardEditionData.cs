using Godot;
using Godot.Collections;
using System.Text;

public partial class CardEditionData
{
    public string uuidEdition;
    public string uuidBase;
    string slug;

    string effect;
    string flavor;

    int rarity;
    string collectorNumber;
    string illustrator;

    public CardData baseData;

    public CardEditionData(Dictionary data)
    {
        Variant temp;

        data.TryGetValue("uuid", out temp);
        uuidEdition = temp.As<string>();

        data.TryGetValue("card_id", out temp);
        uuidBase = temp.As<string>();

        data.TryGetValue("slug", out temp);
        slug = temp.As<string>();

        data.TryGetValue("effect", out temp);
        effect = temp.As<string>();

        data.TryGetValue("flavor", out temp);
        flavor = temp.As<string>();

        rarity = data["rarity"].AsInt32();
        collectorNumber = data["collector_number"].AsString();
        illustrator = data["illustrator"].AsString();

        GD.Print(slug);

    }

    public string GetName()
    {
        return baseData.name;
    }
    public string GetEditionSlug()
    {
        return slug;
    }

    public string GetBaseSlug()
    {
        return baseData.slug;
    }

    public string GetElement()
    {
        return baseData.element;
    }

    public Array<string> GetTypes()
    {
        return baseData.types;
    }

    public Array<string> GetSubtypes()
    {
        return baseData.subtypes;
    }

    public Array<string> GetClasses()
    {
        return baseData.classes;
    }

    public string GetEffect()
    {
        if (effect == null)
            return baseData.effect;
        else
            return effect;
    }

    public string GetFlavor()
    {
        if (flavor == null) 
            return baseData.flavor;
        else
            return flavor;
    }

    public int GetCost()
    {
        if (baseData.costMemory.VariantType == Variant.Type.Nil)
            return baseData.costReserve.AsInt32();
        else
            return baseData.costMemory.AsInt32();
    }

    public Variant GetLevel()
    {
        return baseData.level;
    }

    public Variant GetPower()
    {
        return baseData.power;
    }

    public Variant GetLife()
    {
        return baseData.life;
    }

    public Variant GetDurability()
    {
        return baseData.durability;
    }

    public Variant GetSpeed()
    {
        return baseData.speed;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("UUID Edition : ").Append(uuidEdition).Append("\n");
        sb.Append("UUID Base : ").Append(uuidBase).Append("\n");
        sb.Append("Slug : ").Append(slug).Append("\n");
        sb.Append("Collector Number : ").Append(collectorNumber).Append("\n");
        sb.Append("Illustrator : ").Append(illustrator).Append("\n");
        return sb.ToString();
    }
}
