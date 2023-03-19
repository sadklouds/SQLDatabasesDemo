using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public class FullPersonModel
    {
        public BasicPersonModel BasicInfo { get; set; }
        public List<AddressModel> Addresses { get; set; } = new List<AddressModel> { };

    }
}
