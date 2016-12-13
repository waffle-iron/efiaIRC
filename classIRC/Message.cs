using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace classIRC
{
    [Serializable]
    public class Message
    {
        public String Text { get; private set; }
        public DateTime Time { get; private set; }
        public User User { get; private set; }

        public Message(String text, User user)
        {
            this.Text = text;
            this.User = user;

            this.Time = DateTime.Now;
        }

        public Message(String text, User user, DateTime time) : this(text, user)
        {
            this.Time = time;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
