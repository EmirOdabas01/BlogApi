﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.BLL.Dtos
{
    public class TokenResponseDto
    {
        public required int UserId { get; set; }
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
