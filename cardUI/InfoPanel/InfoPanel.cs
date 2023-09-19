using Godot;
using System;

public partial class InfoPanel : ColorRect
{

    [Export] TextureRect imagePreview;
    CardEditionData card;

    public void SetCard(CardEditionData card)
    {
        this.card = card;
        Update();
    }

    public void Update()
    {
        imagePreview.Texture = GD.Load<CompressedTexture2D>("res://images/" + card.slug + ".png");
    }

}
