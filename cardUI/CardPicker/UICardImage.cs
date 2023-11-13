using Godot;

public partial class UICardImage : TextureButton
{

    CardPicker cardPicker;
    InfoPanel infoPanel;

    public CardEditionData card;
    int posInStack;

    public void Init(CardPicker cardPicker, InfoPanel infoPanel)
    {
        this.cardPicker = cardPicker;
        this.infoPanel = infoPanel;
    }

    public Stack GetStack()
    {
        return cardPicker.openStack;
    }

    public void MouseHovered()
    {
        infoPanel.SetCard(card);
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
        cardPicker.Close();
    }

    public void Disable()
    {
        Disabled = true;
        GetChild<Panel>(0).Visible = true;
    }

}
