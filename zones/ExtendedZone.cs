using Godot;
using Godot.Collections;

public partial class ExtendedZone : Zone
{

    [Export] public int width = 50;
    [Export] public int height = 200;

    [Export] public int cardSpacing = 200; //For estimating required space a cards will take up.  (So two cards don't act like repelling magnets)
    int softBound = 20;

    [Export(PropertyHint.Range, "1,3,")] public int rows = 1;
    float[] rowPosistions;

    public Array<CardInstance> cards = new();

    [ExportGroup("Debug")]
    [Export] bool drawBounds = false;

    //DEBUGGING
    public override void _Process(double delta)
    {
        QueueRedraw();
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        CalcuateRowPosistions();
    }

    private void CalcuateRowPosistions()
    {
        rowPosistions = new float[rows];
        rowPosistions[0] = height * 2 / (rows + 1);
        for (int i = 1; i < rows; i++)
        {
            rowPosistions[i] = rowPosistions[0] * (i + 1);
        }
    }

    public override void _Draw()
    {
        if (drawBounds == false) return;
        DrawRect(new Rect2(-width, -height, width * 2, height * 2), Colors.Green, false);
        DrawCircle(Vector2.Zero, 3, Colors.White);
        DrawLine(new Vector2(-softBound, -height), new Vector2(-softBound, height), Colors.Blue);
        DrawLine(new Vector2(softBound, -height), new Vector2(softBound, height), Colors.Blue);
        foreach (float row in rowPosistions)
            DrawLine(new Vector2(-width, -height + row), new Vector2(width, -height + row), Colors.Red);
    }

    public void UpdateCardSpacing()
    {

        //Update Card Bounds
        int totalSize = cardSpacing * cards.Count;

        switch (cards.Count)
        {
            case 0:
                break;
            case 1:
                cards[0].posGoal = GlobalPosition;
                break;
            default:
                int area;
                softBound = totalSize / 2;
                if (totalSize > (width * 2))
                    area = width * 2;
                else
                    area = totalSize;
                float step = area / (cards.Count - 1);
                Vector2 newPos = GlobalPosition;
                newPos.X -= System.Math.Min(softBound, width);
                foreach (CardInstance card in cards)
                {
                    card.posGoal = newPos;
                    newPos.X += step;
                }
                break;
        }
    }

    public CardInstance GetLastCard(bool remove = false)
    {
        if (remove)
        {
            CardInstance card = cards[cards.Count - 1];
            cards.Remove(card);
            UpdateCardSpacing();
            return card;
        }
        else
        {
            return cards[cards.Count - 1];
        }
    }

    public void AddCard(CardInstance cardInstance)
    {
        cards.Add(cardInstance);
        UpdateCardSpacing();
    }

    public void RemoveCard(CardInstance cardInstance)
    {
        cards.Remove(cardInstance);
        UpdateCardSpacing();
    }

    public void RemoveLastCard()
    {
        cards.RemoveAt(cards.Count - 1);
        UpdateCardSpacing();
    }

}
