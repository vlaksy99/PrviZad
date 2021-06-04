using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public enum EPravaPristupa { CITANJE, AZURIRANJE, REPLIKACIJA};
    public class DirektorijumKorisnika
    {
        Dictionary<string, Korisnik> korisnici = new Dictionary<string, Korisnik>();
        Dictionary<string, string> autentifikovani = new Dictionary<string, string>();
        Dictionary<string, SortedSet<EPravaPristupa>> prava = new Dictionary<string, SortedSet<EPravaPristupa>>();

        public DirektorijumKorisnika()
        {
            DodajKorisnika("admin", "adm1n");
            DodajKorisnika("pera", "p3ra");
            DodajKorisnika("replikator", "repl1kator");

            SortedSet<EPravaPristupa> citanje = new SortedSet<EPravaPristupa>();
            SortedSet<EPravaPristupa> azuriranje = new SortedSet<EPravaPristupa>();
            SortedSet<EPravaPristupa> repliciranje = new SortedSet<EPravaPristupa>();

            citanje.Add(EPravaPristupa.CITANJE);
            prava.Add("pera", citanje);

            azuriranje.Add(EPravaPristupa.AZURIRANJE);
            azuriranje.Add(EPravaPristupa.CITANJE);
            prava.Add("admin", azuriranje);
           
            repliciranje.Add(EPravaPristupa.REPLIKACIJA);
            prava.Add("replikator", repliciranje);


            
        }

        public void DodajKorisnika(string ime, string lozinka)
        {
            korisnici.Add(ime, new Korisnik(ime, KodiranjeLozinke.KodiranTekst(lozinka)));
        }

        public string AutentifikacijaKorisnika(string ime, string lozinka)
        {
            if(korisnici.TryGetValue(ime, out Korisnik k))
            {
                if(KodiranjeLozinke.KodiranTekst(lozinka).Equals(k.Lozinka))
                {
                    k.Autentifikovan = true;
                    string token = KodiranjeLozinke.KodiranTekst(ime);
                    k.Token = token;
                    autentifikovani.Add(token, ime);
                    return token;
                }
            }
            BezbednosniIzuzetak izuzetak = new BezbednosniIzuzetak("Neispravno ime ili lozinka");
            throw new FaultException<BezbednosniIzuzetak>(izuzetak);
        }
        public bool KorisnikAutentifikovan(string token)
        {
            if(autentifikovani.ContainsKey(token))
            {
                return true;
            }
            BezbednosniIzuzetak izuzetak = new BezbednosniIzuzetak("Korisnik nije autentifikovan");
            throw new FaultException<BezbednosniIzuzetak>(izuzetak);
        }

        public bool KorisnikAutorizovan(string token, EPravaPristupa pravo)
        {
            if(autentifikovani.ContainsKey(token) && prava.ContainsKey(autentifikovani[token]) && prava[autentifikovani[token]].Contains(pravo))
            {
                return true;
            }
            BezbednosniIzuzetak izuzetak = new BezbednosniIzuzetak("Korisnik nije autorizovan");
            throw new FaultException<BezbednosniIzuzetak>(izuzetak);
        }
    }
}
