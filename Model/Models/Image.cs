using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Entities.Models
{
    public class Image
    {
        public int Id { get; set; }
        public byte[] ImageData { get; set; } = null!;
        public int PostId { get; set; }
        public Post Post { get; set; } = null!;
    }
}
