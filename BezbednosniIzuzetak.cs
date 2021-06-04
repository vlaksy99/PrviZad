using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class BezbednosniIzuzetak
    {
        string poruka;

        public BezbednosniIzuzetak(string poruka)
        {
            this.Poruka = poruka;
        }

        [DataMember]
        public string Poruka { get => poruka; set => poruka = value; }
    }
}
