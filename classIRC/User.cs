using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace classIRC
{
    [Serializable]
    public class User
    {
        public string Address { get; private set; }
        public String Name { get; private set; }
        public String Id { get; private set;  }

        public User (string address, String name)
        {
            this.Address = address;
            this.Name = name;
            this.Id = generateId(address);
        }

        public String getUniqueName ()
        {
            return Name + "." + Id;
        }

        private String generateId (string addess)
        {
            return null;
        }
    }
}
