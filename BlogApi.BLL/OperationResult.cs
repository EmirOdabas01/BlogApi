using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.BLL
{
    public class OperationResult
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = String.Empty;
    }
}
