using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace TodoApi.Utilities
{
  public class EnumHashSetJsonValueConverter<T> : ValueConverter<HashSet<T>, string> where T : Enum
  {
    // https://gregkedzierski.com/essays/enum-collection-serialization-in-dotnet-core-and-entity-framework-core/
    public EnumHashSetJsonValueConverter() : base(
      v => JsonConvert
        .SerializeObject(v.Select(e => e.ToString()).ToList()),
      v => JsonConvert
        .DeserializeObject<ICollection<string>>(v)
        .Select(e => (T)Enum.Parse(typeof(T), e)).ToHashSet())
    {
    }
  }
}
