using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class BibliotekaIzuzetak
    {

        string razlog;

        public BibliotekaIzuzetak(string razlog)
        {
            this.razlog = razlog;
        }

        [DataMember]
        public string Razlog { get => razlog; set => razlog = value; }
    }
}
