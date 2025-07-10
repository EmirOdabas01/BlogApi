using BlogApi.Entities.Enums;
using BlogApi.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.BLL.Dtos
{
    public class PostDto
    {
        public int? Id { get; set; }
        public PostType PostCategory { get; set; }
        public string Header { get; set; } = String.Empty;
        public List<PostBlockDto> Blocks { get; set; } = null!;
    }
}

