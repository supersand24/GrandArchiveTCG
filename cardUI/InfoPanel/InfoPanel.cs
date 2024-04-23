using Godot;
using System;
using System.Text;

public partial class InfoPanel : Panel
{

    CardEditionData card;
    GameZone zone;

    [Export] TextureRect image;

    [Export] PanelContainer namePanel;
    [Export] RichTextLabel cardName;
    [Export] RichTextLabel cost;
    [Export] RichTextLabel types;
    [Export] RichTextLabel subtypes;

    [Export] PanelContainer effectPanel;
    [Export] RichTextLabel effect;
    [Export] RichTextLabel flavor;

    [Export] PanelContainer statsPanel;
    [Export] RichTextLabel stats;
    [Export] RichTextLabel counters;

    [Export] PanelContainer zonePanel;
    [Export] RichTextLabel zoneName;
    [Export] RichTextLabel zoneCardCount;

    [Export] RichTextLabel debug;

    [Obsolete]
    public void SetCard(CardEditionData card)
    {
        this.card = card;

        StringBuilder sb = new StringBuilder();

        //Image
        image.Texture = GD.Load<CompressedTexture2D>("res://images/" + card.GetEditionSlug() + ".png");

        //Panel Color match Element
        ThemeTypeVariation = card.GetElement();
        namePanel.ThemeTypeVariation = card.GetElement();
        namePanel.Show();
        effectPanel.ThemeTypeVariation = card.GetElement();
        effectPanel.Show();
        statsPanel.ThemeTypeVariation = card.GetElement();
        statsPanel.Show();

        //Name
        cardName.Text = card.GetName();

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

        //Zone
        //Placeholder
        zoneName.Text = "Unknown Zone";
        zoneCardCount.Text = "? Cards";

        //Debug
        debug.Text = card.uuidBase + " | " + card.uuidEdition;
    }

    public void SetStack(CardInstance cardObject)
    {

        StringBuilder sb = new StringBuilder();

        if (cardObject.currentZone.isPrivate)
        {
            zone = cardObject.currentZone;

            image.Texture = GD.Load<CompressedTexture2D>("res://images/cardBack.png");

            ThemeTypeVariation = "NORM";
            namePanel.Hide();
            effectPanel.Hide();
            statsPanel.Hide();

            zoneName.Text = zone.name;
            sb.Append("[right]").Append(cardObject.stack.Count).Append(" Card");
            if (cardObject.stack.Count != 1) if (cardObject.stack.Count != 1) sb.Append("s");
            zoneCardCount.Text = sb.ToString();
        }
        else
        {

        }
    }

}
