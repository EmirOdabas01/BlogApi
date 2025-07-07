using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Entities.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required string PasswordHash { get; set; }

        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public byte[] ProfileImage { get; set; } = null!;
    }
}
