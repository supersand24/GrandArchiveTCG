using Godot;
using System;
using System.Collections.Generic;

public partial class CardInstance : Node2D
{
    [Export] public bool canPickup = false;
    public int owner = 0;
    public int layer = 0;
    [Export] public GameZone currentZone;

    [Export] public Vector2 posGoal = Vector2.Zero;

    protected bool faceUp = false;

    [ExportGroup("Nodes")]
    [Export] protected Sprite2D cardSprite;
    [Export] Sprite2D highlightSprite;
    [Export] protected AnimationPlayer animPlayer;

    //CardSingle
    CardEditionData card = null;

    //CardStack
    public List<CardEditionData> stack = new();

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

    public void SetCard(CardEditionData card)
    {
        cardSprite.Texture = GD.Load<CompressedTexture2D>("res://images/" + card.GetEditionSlug() + ".png");
        this.card = card;
    }

    public void AddCardToTop(CardEditionData card)
    {
        Show();
        stack.Insert(0, card);
    }

    public CardInstance PullTopCard()
    {
        if (stack.Count > 0)
        {
            CardInstance newCard = GD.Load<PackedScene>("res://cardInstance/CardInstance.tscn").Instantiate<CardInstance>();
            GetParent().AddChild(newCard);
            newCard.SetCard(RemoveCardFromTop());
            newCard.Position = Position;
            newCard.posGoal = Position;
            newCard.currentZone = currentZone;
            return newCard;
        }
        else
        {
            GD.PrintErr("No more cards in " + Name);
            return null;
        }
    }

    public CardEditionData RemoveCardFromTop()
    {
        CardEditionData card = stack[0];
        stack.RemoveAt(0);
        return card;
    }

    public void AddCardToBottom(CardEditionData card)
    {
        Show();
        stack.Add(card);
    }

    public void Flip()
    {
        if (faceUp)
            animPlayer.Play("flipDown");
        else
            animPlayer.Play("flipUp");
        faceUp = !faceUp;
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
