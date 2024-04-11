using Godot;
using Godot.Collections;

public partial class CardSingle : CardInstance
{
	CardEditionData card;
	public string uuid { get; set; }

	public void SetCard(string uuid)
	{
		this.uuid = uuid;
		Game game = GetParent().GetOwner<Game>();

		card = game.cardDataManager.GetCardEdition(uuid);

		GD.Print("Trying to load " + "res://images/" + card.GetEditionSlug() + ".png");
		GetNode<Sprite2D>("CardFront").Texture = GD.Load<CompressedTexture2D>("res://images/" + card.GetEditionSlug() + ".png");
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
			if (layer == 0) { }
			//DrawAnim(); //Going to Higher
			else { }
                //DropAnim(); //Going to Lower
        }

        layer = zone.layer;
        //zone.AddCard(this);

        zone.UpdateCardSpacing();
	}

	public void Drop()
	{
		//animPlayer.Play("lower");
		Game game = GetParent().GetOwner<Game>();
		Vector2 oldPos = GlobalPosition;
		GlobalPosition = oldPos;
	}

	public void SetGoals(Vector2 posistion, int index)
	{
		posGoal = posistion;
		ZIndex = (layer * 100) + index;
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
