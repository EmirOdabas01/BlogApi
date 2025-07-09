using BlogApi.Entities.Enums;
using BlogApi.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.BLL.Dtos
{
    public class PostBlockDto
    {
        public int? Id { get; set; }
        public int PostId { get; set; }
        public BlockType BlockCategory { get; set; }
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public int Order { get; set; }
    }
}
