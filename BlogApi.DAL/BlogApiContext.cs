using BlogApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.DAL
{
    public class BlogApiContext : DbContext
    {
        public BlogApiContext(DbContextOptions<BlogApiContext> options) : base(options) { }
       

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}
