using com.libera.models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.libera.tests
{
    public class dbcontext : DbContext
    {
        public DbSet<Coin> Coins { get; set; }
        public DbSet<CoinType> CoinTypes { get; set; }
    }
}
