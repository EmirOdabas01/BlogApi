using BlogApi.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Entities.Models
{
    public class Post
    {
        public int Id { get; set; }
        public Category PostCategory{ get;set; }
        public required string Header { get; set; }
        public required string Content { get; set; }
        public ICollection<Image> Images { get; set; } = new List<Image>();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        public required User User { get; set; }
    }
}
