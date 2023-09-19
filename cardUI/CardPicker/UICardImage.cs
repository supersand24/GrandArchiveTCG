using Godot;

public partial class UICardImage : TextureButton
{

    CardEditionData card;

    public void MouseHovered()
    {
        GetParent().GetParent().GetParent().GetParent().GetParent().GetParent().GetParent<Game>().infoPanel.SetCard(card);
    }

    public void SetCard(CardEditionData card)
    {
        this.card = card;
        TextureNormal = GD.Load<CompressedTexture2D>("res://images/" + card.slug + ".png");
    }

    public void OnPressed()
    {
        GD.Print("Activating " + card.slug);
    }

}
