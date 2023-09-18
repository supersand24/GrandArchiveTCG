using Godot;

public partial class CardPicker : ColorRect
{

    [Export] PackedScene UICard;

    [Export] GridContainer cardGrid;

    public void AddCard(CardEditionData cardEdition)
    {
        UICardImage card = UICard.Instantiate<UICardImage>();
        cardGrid.AddChild(card);
        card.SetCard(cardEdition);
    }

}
