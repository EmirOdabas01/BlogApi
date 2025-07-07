using BlogApi.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Entities.Models
{
    public class PostBlock
    {
        public int Id { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; } = null!;

        public BlockType BlockCategory { get; set; }

        public string? Content { get; set; }
        public string? ImageUrl { get; set; }

        public int Order { get; set; }
    }
}
