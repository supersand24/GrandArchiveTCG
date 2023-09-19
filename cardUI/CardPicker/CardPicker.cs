using Godot;

public partial class CardPicker : ColorRect
{

    [Export] PackedScene UICard;

    [Export] GridContainer cardGrid;

    Stack stack;

    public void SetStack(Stack stack)
    {
        this.stack = stack;
    }

    public Stack GetStack()
    {
        return stack;
    }

    public void AddCard(CardEditionData cardEdition, int index)
    {
        UICardImage card = UICard.Instantiate<UICardImage>();
        cardGrid.AddChild(card);
        card.SetCard(cardEdition, index);
    }

    public void ClearCards()
    {
        foreach (Node card in cardGrid.GetChildren())
        {
            card.QueueFree();
        }
    }

}
