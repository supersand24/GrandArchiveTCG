using Godot;
using Godot.Collections;
using System.Text;

public partial class CardDatabaseResponse
{

    public int page = 0;
    int totalCards = 0;
    int paginatedCardsCount = 0;
    int pageSize = 0;
    public bool hasMore = false;
    public int totalPages = 0;
    string sort = "";
    string order = "";

    public Array data;

    public CardDatabaseResponse(Godot.Collections.Dictionary dictionary)
    {
        Variant temp;
        dictionary.TryGetValue("page", out temp);
        page = temp.AsInt32();

        dictionary.TryGetValue("total_cards", out temp);
        totalCards = temp.AsInt32();

        dictionary.TryGetValue("total_cards", out temp);
        paginatedCardsCount = temp.AsInt32();

        dictionary.TryGetValue("page_size", out temp);
        pageSize = temp.AsInt32();

        dictionary.TryGetValue("has_more", out temp);
        hasMore = temp.AsBool();

        dictionary.TryGetValue("total_pages", out temp);
        totalPages = temp.AsInt32();

        dictionary.TryGetValue("sort", out temp);
        sort = temp.AsString();

        dictionary.TryGetValue("order", out temp);
        order = temp.AsString();

        dictionary.TryGetValue("data", out temp);
        data = temp.AsGodotArray();

    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Page : ").Append(page).Append("\n");
        sb.Append("Total Cards : ").Append(totalCards).Append("\n");
        sb.Append("Paginated Cards Count : ").Append(paginatedCardsCount).Append("\n");
        sb.Append("Page Size : ").Append(pageSize).Append("\n");
        sb.Append("Has More : ").Append(hasMore).Append("\n");
        sb.Append("Total Pages : ").Append(totalPages).Append("\n");
        sb.Append("Sort : ").Append(sort).Append("\n");
        sb.Append("Order : ").Append(order).Append("\n");
        return sb.ToString();
    }
}