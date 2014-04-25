namespace Rush
{
    using System.Collections.Generic;
    using System.Linq;

    public static class DataEx
    {
        public static IDictionary<string, object> AsDictionary(this RushObject obj)
        {
            return obj.GetProperties(true).ToDictionary(p => p.PropertyName, p => p.Value);
        }
    }
}
