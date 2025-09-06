using System.Globalization;

namespace Corp.Pagination;

public static class PagedQueryExtensions
{
    public const int NoQueryLimit = 0;
    public const int DefaultQueryLimit = 1000;
    private const int DefaultSearchTermLimit = 3;

    public static IQueryable<T> Paginate<T>(this IOrderedQueryable<T> queryable, PagedQuery query, out int count, int max = DefaultQueryLimit, Dictionary<string, string>? viewToDataColumnMap = null)
    {
        // Query the count prior to applying skip/take.
        // http://stackoverflow.com/a/4284469/51558
        count = queryable.Count();

        if (query == null)
        {
            return queryable;
        }

        //  take    max     limit
        //  0       100     100
        //  50      100     50
        //  150     100     100
        var modifiedQuery = queryable.Skip(query.Skip);
        if (max != NoQueryLimit)
        {
            modifiedQuery = modifiedQuery.Take(query.Take > 0 ? Math.Min(query.Take, max) : max);
        }

        return modifiedQuery;
    }

    public static void Search(this PagedQuery query, Action<string> addTerm, int limit = DefaultSearchTermLimit)
    {
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            string[] terms = query.Search
                .ToLower(CultureInfo.CurrentCulture)
                .Split(' ')
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrEmpty(s))
                .ToArray();

            foreach (string term in terms)
            {
                if (--limit < 0)
                    break;

                addTerm(term);
            }
        }
    }
}
