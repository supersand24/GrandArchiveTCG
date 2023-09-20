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

        //Image
        image.Texture = GD.Load<CompressedTexture2D>("res://images/" + card.GetEditionSlug() + ".png");

        //Panel Color match Element
        ThemeTypeVariation = card.GetElement();
        name.GetParent().GetParent().GetParent<PanelContainer>().ThemeTypeVariation = card.GetElement();
        effect.GetParent().GetParent<PanelContainer>().ThemeTypeVariation = card.GetElement();
        stats.GetParent().GetParent<PanelContainer>().ThemeTypeVariation = card.GetElement();

        //Name
        name.Text = card.GetName();

        //Cost
        cost.Text = "[right]" + card.GetCost() + " COST";

        //Type
        foreach (string type in card.GetTypes()) sb.Append(type).Append(" ");
        types.Text = sb.ToString();
        sb.Clear();

        //SubType
        sb.Append("[right]");
        foreach (string subtype in card.GetSubtypes()) sb.Append(subtype).Append(" ");
        subtypes.Text = sb.ToString();
        sb.Clear();

        //Effect
        effect.Text = card.baseData.effectFormatted;

        //Flavor
        flavor.Text = "[i]" + card.GetFlavor();

        //Stats
        stats.Text = "[right]" + card.GetLife() + " Life";

        //Counters
        counters.Text = "";

        //Debug
        debug.Text = card.uuidBase + " | " + card.uuidEdition;
        
    }

}
