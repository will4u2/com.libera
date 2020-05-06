using com.libera.core;
using Dapper.Contrib.Extensions;

namespace com.libera.models.Entities
{
    public partial class CoinType : BaseClass
    {
        public string Type { get; set; }
        public decimal Value { get; set; }
    }
}
