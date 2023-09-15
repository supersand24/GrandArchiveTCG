using Godot;
using Godot.Collections;

public partial class Hand : Node2D
{
    [Export] PackedScene cardInstance { get; set; }
    public Array<CardInstance> cards = new();

    [Export] Stack mainDeck;

    [Export] int bounds = 300;

    public override void _Ready()
    {
        for (int i = 0; i < 5; i++)
        {
            cards.Add(mainDeck.DrawCard());
        }
        UpdateHandSpacing();
    }

    public void UpdateHandSpacing()
    {
        //Array<Node> cards = GetChildren();
        if (cards.Count > 1)
        {
            float step = bounds*2/(cards.Count-1);
            Vector2 newPos = GlobalPosition;
            newPos.X -= bounds;
            foreach (CardInstance card in cards)
            {
                card.posGoal = newPos;
                newPos.X += step;
            }
        }
        else
        {
            CardInstance card = GetChild<CardInstance>(0);
            card.posGoal = GlobalPosition;
        }

    }
}