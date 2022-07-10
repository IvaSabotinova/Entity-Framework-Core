using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace FastFood.Data
{
    public class FastFoodContextDesignTimeFactory : IDesignTimeDbContextFactory<FastFoodContext>
    {
        public FastFoodContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<FastFoodContext>();
            builder.UseSqlServer("Server=.;Database=FastFood;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new FastFoodContext(builder.Options);
        }
    }
}
