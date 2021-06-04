using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Korisnik
    {
        string korisnickoIme;
        string lozinka;
        string token;
        bool autentifikovan;
        HashSet<EPravaPristupa> prava;

        public Korisnik(string korisnickoIme, string lozinka)
        {
            this.KorisnickoIme = korisnickoIme;
            this.Lozinka = lozinka;
            this.Token = "";
            this.Autentifikovan = false;
            this.Prava = new HashSet<EPravaPristupa>();
        }

        public string KorisnickoIme { get => korisnickoIme; set => korisnickoIme = value; }
        public string Lozinka { get => lozinka; set => lozinka = value; }
        public string Token { get => token; set => token = value; }
        public bool Autentifikovan { get => autentifikovan; set => autentifikovan = value; }
        public HashSet<EPravaPristupa> Prava { get => prava; set => prava = value; }
    }
}
