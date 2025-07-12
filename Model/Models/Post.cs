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
        public PostType PostCategory{ get;set; }
        public string Header { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<PostBlock> Blocks { get; set; } = new List<PostBlock>();
    }
}
