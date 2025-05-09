using Apilogin.Models;
using System.Collections.Generic;

namespace Apilogin.Data
{
    public class InMemoryDbContext
    {
        public List<User> Users { get; set; } = new List<User>();
    }
}
