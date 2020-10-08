namespace CycleFinder.Models
{
    public class Symbol
    {
        public string Name { get; }
        public string QuoteAsset { get; }

        public Symbol(string name, string quoteAsset)
        {
            Name = name;
            QuoteAsset = quoteAsset;
        }
    }
}
