using Godot;
using System;

public partial class CardInstance : Node2D
{
    [Export] public bool canPickup = false;
    public int owner = 0;
    public int layer = 0;
    [Export] public GameZone currentZone;

    [Export] public Vector2 posGoal = Vector2.Zero;

    bool faceUp = false;

    [ExportGroup("Nodes")]
    [Export] Sprite2D cardSprite;
    [Export] Sprite2D highlightSprite;
    [Export] AnimationPlayer animPlayer;

    public override void _Process(double delta)
    {
        MoveToGoal(10f * (float)delta);
    }

    public void MouseHovered()
    {
        GetTree().Root.GetChild<Game>(0).infoPanel.SetStack(this);
    }

    public void InputEvent(Node viewport, InputEvent input, int shape_idx)
    {
        if (input.IsActionPressed("left_click"))
        {
            Game game = GetTree().Root.GetChild<Game>(0);
            if (game.highlighted == null)
            {
                Highlight();
                game.UnhighlightStack();
                game.highlighted = this;
            }
            else
            {
                game.UnhighlightStack();
            }
        }
    }

    public void Highlight()
    {
        highlightSprite.Show();
    }

    public void Unhighlight()
    {
        highlightSprite.Hide();
    }

    public void MoveToGoal(float speed)
    {
        Position = Lerp(Position, posGoal, speed);
    }

    private Vector2 Lerp(Vector2 beginning, Vector2 goal, float speed)
    {
        return beginning * (1 - speed) + goal * speed;
    }

}
