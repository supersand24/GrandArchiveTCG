using Godot;

public partial class UICardImage : TextureButton
{

    CardEditionData card;
    int posInStack;

    public Stack GetStack()
    {
        return GetParent().GetParent().GetParent().GetParent().GetParent<CardPicker>().GetStack();
    }

    public void MouseHovered()
    {
        GetParent().GetParent().GetParent().GetParent().GetParent().GetParent().GetParent<Game>().infoPanel.SetCard(card);
    }

    public void SetCard(CardEditionData card, int index)
    {
        this.card = card;
        posInStack = index;
        TextureNormal = GD.Load<CompressedTexture2D>("res://images/" + card.slug + ".png");
    }

    public void OnPressed()
    {
        GD.Print("Activating " + card.slug);
        //GetStack().SpawnCard(posInStack);
        GetStack().ActivateCard(posInStack);
        GetParent().GetParent().GetParent().GetParent().GetParent().GetParent().GetParent<Game>().CloseCardPicker();
    }

}
