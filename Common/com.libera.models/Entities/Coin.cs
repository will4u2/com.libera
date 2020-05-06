using com.libera.core;
using Dapper.Contrib.Extensions;

namespace com.libera.models.Entities
{
    public partial class Coin : BaseClass
    {
        public long CoinTypeId { get; set; }
        public int Quantity { get; set; }
        [Write(false)]
        public virtual CoinType Type { get; set; }
    }
}
