using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TodoApi.Utilities
{
  public class HashSetValueComparer<T> : ValueComparer<HashSet<T>>
  {
    // https://gregkedzierski.com/essays/enum-collection-serialization-in-dotnet-core-and-entity-framework-core/
    #pragma warning disable CS8602
    #pragma warning disable CS8604
    public HashSetValueComparer() : base((c1, c2) => c1.SequenceEqual(c2),
      c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())), c => (HashSet<T>)c)
    {
    }
    #pragma warning restore CS8602
    #pragma warning restore CS8604
  }
}
