using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Server
    {
        public int ID {get;set;}
        public string Name { get; set; }
        public string HostUsername { get; set; }
        public string OfferData { get; set; }
    }
    public class Servers : List<Server>
    {

    }
}
