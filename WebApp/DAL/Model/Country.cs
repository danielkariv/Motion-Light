using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Countries : List<Country>
    {

    }
}
