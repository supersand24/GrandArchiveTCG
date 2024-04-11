using Godot;
using Godot.Collections;

public partial class CardInstance : Node2D
{
	CardEditionData card;
	public string uuid { get; set; }

	public bool canPickup = true;
	public int ownerNumber = 0;
	public int layer = 0;

	Vector2 posGoal = Vector2.Zero;

	public AnimationPlayer animPlayer;
	[Export] bool faceUp = true;

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

		card = game.cardDataManager.GetCardEdition(uuid);

		GD.Print("Trying to load " + "res://images/" + card.GetEditionSlug() + ".png");
		GetNode<Sprite2D>("CardFront").Texture = GD.Load<CompressedTexture2D>("res://images/" + card.GetEditionSlug() + ".png");
	}

	public void DrawAnim()
	{
		animPlayer.Play("draw");
	}

    public void DropAnim()
    {
        animPlayer.Play("lower");
    }

    public void FlipUp()
	{
		faceUp = true;
		animPlayer.Play("flipUp");
	}

	public void MoveToZone(ExtendedZone zone)
	{

		GD.Print("Moving " + GetDebugName() + " to " + zone.Name);

        if (zone.layer == layer)
        {
			//Same Layer
			GD.Print("Same Layer");
        }
        else
        {
            //Different Layer
            if (layer == 0)
                DrawAnim(); //Going to Higher
            else
                DropAnim(); //Going to Lower
        }

        layer = zone.layer;
        //zone.AddCard(this);

        zone.UpdateCardSpacing();
	}

	public void MoveToZone(Stack zone)
	{

	}

	public void InputEvent(Node viewport, InputEvent input, int shape_idx)
	{
		if (input.IsActionPressed("left_click") && canPickup)
		{
			Game game = GetParent().GetOwner<Game>();
			game.grabbedCard = this;
		}
	}

	public void MouseHovered()
	{
		GetParent().GetParent<Game>().infoPanel.SetCard(card);
	}

	public void Drop()
	{
		animPlayer.Play("lower");
		Game game = GetParent().GetOwner<Game>();
		Vector2 oldPos = GlobalPosition;
		GlobalPosition = oldPos;
	}

	public void SetGoals(Vector2 posistion, int index)
	{
		posGoal = posistion;
		ZIndex = (layer * 100) + index;
	}

	public void MoveToGoal(float speed)
	{
		GlobalPosition = Lerp(GlobalPosition, posGoal, speed);
	}

	private Vector2 Lerp(Vector2 beginning, Vector2 goal, float speed)
	{
		return beginning * (1 - speed) + goal * speed;
	}

	//----------------------------------------------------------------------------------
	// CARD DATA RELATED
	//----------------------------------------------------------------------------------

	public CardEditionData GetCardEditionData()
	{
		Game game = GetParent().GetOwner<Game>();
		return game.cardDataManager.GetCardEdition(uuid);
	}

	public string GetDebugName()
	{
		return GetCardName() + " Card";
	}

	public string GetCardName()
	{
		CardEditionData cardData = GetCardEditionData();
		return cardData.GetName();
	}

	public int GetCardCost()
	{
		CardEditionData cardData = GetCardEditionData();
		return cardData.GetCost();
	}

	public Array<string> GetCardTypes()
	{
		CardEditionData cardData = GetCardEditionData();
		return cardData.GetTypes();
	}

}
