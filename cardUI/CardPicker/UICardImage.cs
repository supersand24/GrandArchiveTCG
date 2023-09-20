using Godot;

public partial class UICardImage : TextureButton
{

    public CardEditionData card;
    int posInStack;

    public Stack GetStack()
    {
        return GetParent().GetParent().GetParent().GetParent().GetParent<CardPicker>().openStack;
    }

    public void MouseHovered()
    {
        GetParent().GetParent().GetParent().GetParent().GetParent().GetParent().GetParent<Game>().infoPanel.SetCard(card);
    }

    public void SetCard(CardEditionData card, int index)
    {
        this.card = card;
        posInStack = index;
        TextureNormal = GD.Load<CompressedTexture2D>("res://images/" + card.GetEditionSlug() + ".png");
    }

    public void OnPressed()
    {
        GD.Print("Activating " + card.GetEditionSlug());
        GetStack().ActivateCard(posInStack);
        GetParent().GetParent().GetParent().GetParent().GetParent().GetParent().GetParent<Game>().cardPicker.Close();
    }

    public void Disable()
    {
        Disabled = true;
        GetChild<Panel>(0).Visible = true;
    }

}
