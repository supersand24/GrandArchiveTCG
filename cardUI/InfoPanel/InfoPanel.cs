using Godot;
using System;

public partial class InfoPanel : ColorRect
{

    [Export] TextureRect imagePreview;

    public void SetCard(Texture2D image)
    {
        imagePreview.Texture = image;
    }

}
