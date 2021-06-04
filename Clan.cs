using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Clan
    {
        string ime;
        string prezime;
        long jmbg;
        List<string> knjige = new List<string>();

        public Clan()
        {
            this.Ime = "";
            this.Prezime = "";
            this.Jmbg = 0;
        }

        public Clan(string ime, string prezime, long jmbg)
        {
            this.ime = ime;
            this.prezime = prezime;
            this.jmbg = jmbg;
            this.knjige = new List<string>();
        }

        [DataMember]
        public string Ime { get => ime; set => ime = value; }
        [DataMember]
        public string Prezime { get => prezime; set => prezime = value; }
        [DataMember]
        public long Jmbg { get => jmbg; set => jmbg = value; }
        [DataMember]
        public List<string> Knjige { get => knjige; set => knjige = value; }

        public override string ToString()
        {
            string s = $"JMBG: {Jmbg} Ime: {Ime} Prezime: {Prezime}";
            Console.WriteLine();
            for(int i = 0; i < knjige.Count; i++)
            {
                s += knjige[i];
            }

            return s;
        }
    }
}
