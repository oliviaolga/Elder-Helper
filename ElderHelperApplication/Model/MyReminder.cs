using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElderHelperApplication.Model
{
    [DataContract]
    public class MyReminder
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public TimeSpan TimeOfDay { get; set; }

        [DataMember]
        public string Describe { get; set; }

        [DataMember]
        public string audio { get; set; }

        [DataMember]
        public DayOfWeek[] DaysOfWeek { get; set; }

        public bool IsOneTime()
        {
            return SingleFireTime != DateTime.MinValue;
        }

        [DataMember]
        public DateTime SingleFireTime { get; set; }       
                
        public bool oneMonth = false;

        public bool everyday = false;
    }
}
