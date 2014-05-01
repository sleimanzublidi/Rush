namespace Rush
{
    using System.Collections.Generic;
    using System.Linq;

    public static class DataEx
    {
        public static IDictionary<string, object> AsDictionary(this RushObject obj)
        {
            return obj.GetPropertyValues().ToDictionary(p => p.Key, p => p.Value);
        }
    }
}
