using Godot;

public partial class CardInstance : Node2D
{

	public Vector2 posGoal = Vector2.Zero;

	public AnimationPlayer animPlayer;
	[Export] bool faceUp = true;

	public string uuid { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		animPlayer = GetNode<AnimationPlayer>("Animations");
		if (faceUp) animPlayer.Play("flipUp");
    }

	public void SetCard(string uuid)
	{
		this.uuid = uuid;
		Game game = GetParent().GetOwner<Game>();
		GD.Print(uuid);
		CardEditionData cardEdition = game.cardDataManager.GetCardEdition(uuid);
		GD.Print("Trying to load " + "res://images/" + cardEdition.slug + ".png");
		GetNode<Sprite2D>("CardFront").Texture = GD.Load<CompressedTexture2D>("res://images/" + cardEdition.slug + ".png");
	}

	public void DrawAnim()
	{
		animPlayer.Play("draw");
	}

	public void FlipUp()
	{
		faceUp = true;
		animPlayer.Play("flipUp");
	}

	public void InputEvent(Node viewport, InputEvent input, int shape_idx)
	{
		if (input.IsActionPressed("click"))
		{
			Game game = GetParent().GetOwner<Game>();
			game.grabbedCard = this;
		}
	}

	public void MouseHovered()
	{
		GD.Print("Hello!");
	}

	public void Drop()
	{
		Game game = GetParent().GetOwner<Game>();
		Vector2 oldPos = GlobalPosition;
		GlobalPosition = oldPos;
    }

	public void MoveToGoal(float speed)
	{
        GlobalPosition = Lerp(GlobalPosition, posGoal, speed);
    }

    private Vector2 Lerp(Vector2 beginning, Vector2 goal, float speed)
    {
        return beginning * (1 - speed) + goal * speed;
    }

}
