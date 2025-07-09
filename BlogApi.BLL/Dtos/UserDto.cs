﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.BLL.Dtos
{
    public class UserDto
    {
        public int? Id { get; set; }
        public string UserName { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;

    }
}
