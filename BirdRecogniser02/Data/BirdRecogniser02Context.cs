using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BirdRecogniser02.Models;

namespace BirdRecogniser02.Data
{
    public class BirdRecogniser02Context : DbContext
    {
        public BirdRecogniser02Context (DbContextOptions<BirdRecogniser02Context> options)
            : base(options)
        {
        }

        public DbSet<BirdRecogniser02.Models.Submission> Submission { get; set; } = default!;
    }
}
