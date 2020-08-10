using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PizzaStore.Storing;
using Xunit;

namespace PizzaStore.Testing {
    public class UnitTest1 {
      private static readonly SqliteConnection _connection = new SqliteConnection("Data Source=:memory:");
      private static readonly DbContextOptions<PizzaStoreDbContext> _options = new DbContextOptionsBuilder<PizzaStoreDbContext>().UseSqlite(_connection).Options;
      [Fact]
      public void Test1() {
      }
    }
}
