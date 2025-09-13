namespace EFDeepQueryDynamicLinq
{
    public class SortInput : List<KeyValuePair<string, SortDirection>>
    {
        public SortInput Add(string key, SortDirection direction = SortDirection.Asc)
        {
            Add(new KeyValuePair<string, SortDirection>(key, direction));
            return this;
        }
    }

    public enum SortDirection
    {
        Asc,
        Desc
    }
}
