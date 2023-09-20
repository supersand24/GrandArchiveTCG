using Godot;
using Godot.Collections;

public partial class CardPicker : ColorRect
{

    [Export] PackedScene UICard;

    [Export] RichTextLabel title;
    [Export] GridContainer cardGrid;

    public Stack openStack;

    //Limits
    int minLevel = 0;
    int maxLevel = 3;
    Array types = new();

    public void Open(Array<string> uuidList, Stack stack, Dictionary limits = null)
    {
        this.openStack = stack;
        title.Text = stack.Name;

        int i = 0;
        for (i = 0; i < uuidList.Count; i++)
        {
            UICardImage card = UICard.Instantiate<UICardImage>();
            cardGrid.AddChild(card);
            card.SetCard(GetParent().GetParent<Game>().cardDataManager.GetCardEdition(uuidList[i]), i);
        }

        SetLimits(limits);

        Visible = true;
    }

    public void Close()
    {
        Visible = false;
        foreach (Node card in cardGrid.GetChildren())
            card.QueueFree();
        ResetLimits();
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
            if (card.data.level.VariantType != Variant.Type.Nil && card.Disabled == false)
            {
                int cardLevel = card.data.level.AsInt32();
                if (cardLevel < minLevel || cardLevel > maxLevel) card.Disable();
            }

            //If not already disabled, disable if not in Types Array.
            if (card.Disabled == false)
            {
                foreach (string type in types) if (!card.data.types.Contains(type)) card.Disable();
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
