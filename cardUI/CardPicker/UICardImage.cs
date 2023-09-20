using Godot;

public partial class UICardImage : TextureButton
{

    public CardData data;
    CardEditionData edition;
    int posInStack;

    public Stack GetStack()
    {
        return GetParent().GetParent().GetParent().GetParent().GetParent<CardPicker>().openStack;
    }

    public void MouseHovered()
    {
        GetParent().GetParent().GetParent().GetParent().GetParent().GetParent().GetParent<Game>().infoPanel.SetCard(edition);
    }

    public void SetCard(CardEditionData edition, int index)
    {
        this.edition = edition;
        this.data = GetParent().GetParent().GetParent().GetParent().GetParent().GetParent().GetParent<Game>().cardDataManager.GetCardData(edition.uuidBase);
        posInStack = index;
        TextureNormal = GD.Load<CompressedTexture2D>("res://images/" + edition.slug + ".png");
    }

    public void OnPressed()
    {
        GD.Print("Activating " + edition.slug);
        GetStack().ActivateCard(posInStack);
        GetParent().GetParent().GetParent().GetParent().GetParent().GetParent().GetParent<Game>().cardPicker.Close();
    }

}
