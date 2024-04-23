using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class CardPicker : ColorRect
{

    [Export] Game game;

    [Export] PackedScene UICard;

    [Export] RichTextLabel title;
    [Export] GridContainer cardGrid;

    public CardInstance openStack;

    //Limits
    int minLevel = 0;
    int maxLevel = 3;
    Godot.Collections.Array types = new();

    public void Open(List<Card> dataList, CardInstance stack, Dictionary limits = null)
    {
        openStack = stack;
        title.Text = stack.currentZone.name;

        for (int i = 0; i < dataList.Count; i++)
        {
            UICardImage card = UICard.Instantiate<UICardImage>();
            card.Init(this, game.infoPanel);
            cardGrid.AddChild(card);
            card.SetCard(dataList[i].GetData(), i);
        }

        SetLimits(limits);

        Show();
    }

    public void Close()
    {
        Hide();
        foreach (Node card in cardGrid.GetChildren())
            card.QueueFree();
        ResetLimits();
    }
    
    public bool IsOpen()
    {
        return Visible;
    }

    private void SetLimits(Dictionary limits)
    {
        if (limits == null) return;

        foreach (string limit in limits.Keys)
        {
            switch (limit)
            {
                case "level":
                    string code = limits["level"].AsString();
                    if (code.StartsWith("==")) { minLevel = int.Parse(code.Substring("==".Length)); maxLevel = minLevel; }
                    break;
                case "types": types = limits["types"].AsGodotArray(); break;
            }
        }

        foreach (UICardImage card in cardGrid.GetChildren())
        {
            //If not already disabled, disable if out of Level Range.
            if (card.card.GetLevel().VariantType != Variant.Type.Nil && card.Disabled == false)
            {
                int cardLevel = card.card.GetLevel().AsInt32();
                if (cardLevel < minLevel || cardLevel > maxLevel) card.Disable();
            }

            //If not already disabled, disable if not in Types Array.
            if (card.Disabled == false)
            {
                foreach (string type in types) if (!card.card.GetTypes().Contains(type)) card.Disable();
            }
            
        }

    }

    private void ResetLimits()
    {
        minLevel = 0;
        maxLevel = 3;
        types.Clear();
    }

}
