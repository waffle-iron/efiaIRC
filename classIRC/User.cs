using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace classIRC
{
    [Serializable]
    public class User
    {
        public string Address { get; private set; }
        public String Name { get; private set; }
        public Guid Id { get; private set; }

        public User(string address, String name)
        {
            this.Address = address;
            this.Name = CheckName(name);
            this.Id = GenerateId(name, address);
        }

        public String GetUniqueName()
        {
            return Name + "." + Id;
        }

        private static Guid GenerateId(string name, string address)
        {
            StringBuilder seed = new StringBuilder(name);
            seed.Append(address);
            seed.Append(GetRandomHex(8));
            Guid guid = new Guid(seed.ToString());
            return guid;
        }

        private static string CheckName(string name)
        {
            if (Regex.IsMatch(name, @"^[a-zA-Z0-9]{4,}"))
            {
                return name;
            }
            else
            {
                throw new InvalidUserNameException();
            }
        }

        private static string GetRandomHex(int digits)
        {
            Random random = new Random();
            byte[] buffer = new byte[digits / 2];
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            if (digits % 2 == 0) return result;
            return result + random.Next(16).ToString("X");
        }
    }
}

public class InvalidUserNameException : Exception
{
    public InvalidUserNameException() { }
    public InvalidUserNameException(string message) : base(message) { }
    public InvalidUserNameException(string message, Exception inner) : base(message, inner) { }
}