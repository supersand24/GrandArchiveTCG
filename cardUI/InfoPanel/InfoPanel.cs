using Godot;
using System;
using System.Text;

public partial class InfoPanel : Panel
{

    CardEditionData card;

    [Export] TextureRect image;

    [Export] RichTextLabel name;
    [Export] RichTextLabel cost;

    [Export] RichTextLabel types;
    [Export] RichTextLabel subtypes;

    [Export] RichTextLabel effect;
    [Export] RichTextLabel flavor;

    [Export] RichTextLabel stats;
    [Export] RichTextLabel counters;

    [Export] RichTextLabel debug;

    public void SetCard(CardEditionData card)
    {
        this.card = card;
        Update();
    }

    public void Update()
    {
        StringBuilder sb = new StringBuilder();

        CardData data = GetParent().GetParent<Game>().cardDataManager.GetCardData(card.uuidBase);

        //Image
        image.Texture = GD.Load<CompressedTexture2D>("res://images/" + card.slug + ".png");

        if (data == null) { GD.PrintErr("Info Panel could not get Card Data!"); return; }

        //Panel Color match Element
        ThemeTypeVariation = data.element;

        //Name
        name.Text = data.name;

        //Cost
        if (data.costReserve.VariantType == Variant.Type.Nil)
            cost.Text = "[right]" + data.costMemory + " COST";
        else
            cost.Text = "[right]" + data.costReserve + " COST";

        //Type
        foreach (string type in data.types) sb.Append(type).Append(" ");
        types.Text = sb.ToString();
        sb.Clear();

        //SubType
        sb.Append("[right]");
        foreach (string subtype in data.subtypes) sb.Append(subtype).Append(" ");
        subtypes.Text = sb.ToString();
        sb.Clear();

        //Effect
        effect.Text = data.effectFormatted;

        //Flavor
        flavor.Text = "[i]" + data.flavor;

        //Stats
        stats.Text = "[right]" + data.life + " Life";

        //Counters
        counters.Text = "";

        //Debug
        debug.Text = card.uuidBase + " | " + card.uuidEdition;
        
    }

}
