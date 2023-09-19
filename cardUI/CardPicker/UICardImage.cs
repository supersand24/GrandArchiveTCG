using Godot;

public partial class UICardImage : TextureRect
{

    CardEditionData card;

    public void MouseHovered()
    {
        GetParent().GetParent().GetParent().GetParent().GetParent().GetParent<Game>().infoPanel.SetCard(Texture);
    }

    public void SetCard(CardEditionData card)
    {
        this.card = card;
        //GD.Print(card);
        Texture = GD.Load<CompressedTexture2D>("res://images/" + card.slug + ".png");
    }

}
