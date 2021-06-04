using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class BibliotekaServis : IBiblioteka, IBezbednosniMehanizmi
    {
        static readonly DirektorijumKorisnika direktorijum = new DirektorijumKorisnika();
        public string Autentifikacija(string korisnickoIme, string lozinka)
        {
            return direktorijum.AutentifikacijaKorisnika(korisnickoIme, lozinka);
        }

        public bool DobaviClana(string token, long jmbg, out Clan clan)
        {
            direktorijum.KorisnikAutentifikovan(token);
            direktorijum.KorisnikAutorizovan(token, EPravaPristupa.CITANJE);

            if(BazaPodataka.clanovi.ContainsKey(jmbg))
            {
                clan = BazaPodataka.clanovi[jmbg];
                return true;
            }
            else
            {
                BibliotekaIzuzetak izuzetak = new BibliotekaIzuzetak("Ne postoji trazeni clan");
                throw new FaultException<BibliotekaIzuzetak>(izuzetak);
            }
        }

        public void DodajClana(string token, Clan clan)
        {
            direktorijum.KorisnikAutentifikovan(token);
            direktorijum.KorisnikAutorizovan(token, EPravaPristupa.AZURIRANJE);

            if (!BazaPodataka.clanovi.ContainsKey(clan.Jmbg))
            {
                BazaPodataka.clanovi.Add(clan.Jmbg, clan);
            }
            else
            {
                BibliotekaIzuzetak izuzetak = new BibliotekaIzuzetak("Postoji vec clan sa datim JMBGom");
                throw new FaultException<BibliotekaIzuzetak>(izuzetak);

            }
        }

        public bool DodajKnjiguClanu(string token, long jmbg, params string[] knjige)
        {
            direktorijum.KorisnikAutentifikovan(token);
            direktorijum.KorisnikAutorizovan(token, EPravaPristupa.AZURIRANJE);

            if (BazaPodataka.clanovi.ContainsKey(jmbg))
            {
                foreach (string knjiga in knjige)
                {
                    BazaPodataka.clanovi[jmbg].Knjige.Add(knjiga);
                }
                return true;
            }
            else
            {
                BibliotekaIzuzetak izuzetak = new BibliotekaIzuzetak("Ne postoji trazeni clan, nemoguce dodavanje knjiga");
                throw new FaultException<BibliotekaIzuzetak>(izuzetak);
            }
            
        }

        public void IzbrisiClana(string token, long jmbg)
        {
            direktorijum.KorisnikAutentifikovan(token);
            direktorijum.KorisnikAutorizovan(token, EPravaPristupa.AZURIRANJE);

            if (BazaPodataka.clanovi.ContainsKey(jmbg))
            {
                BazaPodataka.clanovi.Remove(jmbg);
            }
            else
            {
                BibliotekaIzuzetak izuzetak = new BibliotekaIzuzetak("Nemoguce brisanje nepostojeceg clana");
                throw new FaultException<BibliotekaIzuzetak>(izuzetak);
            }
        }

        public void IzmeniClana(string token, Clan clan)
        {
            direktorijum.KorisnikAutentifikovan(token);
            direktorijum.KorisnikAutorizovan(token, EPravaPristupa.AZURIRANJE);

            if (BazaPodataka.clanovi.ContainsKey(clan.Jmbg))
            {
                BazaPodataka.clanovi[clan.Jmbg] = clan;
            }
            else
            {
                BibliotekaIzuzetak izuzetak = new BibliotekaIzuzetak("Nemoguce mjenjaje datog clana");
                throw new FaultException<BibliotekaIzuzetak>(izuzetak);
            }
            
        }

        public bool ObrisiKnjiguClanu(string token, long jmbg, params string[] knjige)
        {
            direktorijum.KorisnikAutentifikovan(token);
            direktorijum.KorisnikAutorizovan(token, EPravaPristupa.AZURIRANJE);

            if (BazaPodataka.clanovi.ContainsKey(jmbg))
            {
                foreach (string knjiga in knjige)
                {
                    if (BazaPodataka.clanovi[jmbg].Knjige.Contains(knjiga))
                    {
                        BazaPodataka.clanovi[jmbg].Knjige.Remove(knjiga);
                    }
                    BibliotekaIzuzetak izuzetak1 = new BibliotekaIzuzetak("Ne postoji data knjiga, nemoguce brisanje knjige");
                    throw new FaultException<BibliotekaIzuzetak>(izuzetak1);
                }
                return true;
            }
            else
            {
                BibliotekaIzuzetak izuzetak = new BibliotekaIzuzetak("Ne postoji trazeni clan, nemoguce brisanje knjiga");
                throw new FaultException<BibliotekaIzuzetak>(izuzetak);
            }
            
        }

        public void PosaljiBazu(string token, Dictionary<long, Clan> baza) //UPISI
        {
            direktorijum.KorisnikAutentifikovan(token);
            direktorijum.KorisnikAutorizovan(token, EPravaPristupa.REPLIKACIJA);

            Console.WriteLine("Upis iniciran");
            foreach(Clan c in baza.Values)
            {
                BazaPodataka.clanovi[c.Jmbg] = c;
            }
        }

        public Dictionary<long, Clan> PreuzmiBazu(string token)  //PROCITAJ
        {
            direktorijum.KorisnikAutentifikovan(token);
            direktorijum.KorisnikAutorizovan(token, EPravaPristupa.REPLIKACIJA);

            Console.WriteLine("Citanje inicirano");
            return BazaPodataka.clanovi;
        }

        public List<Clan> SviClanovi(string token)
        {
            direktorijum.KorisnikAutentifikovan(token);
            direktorijum.KorisnikAutorizovan(token, EPravaPristupa.CITANJE);

            return BazaPodataka.clanovi.Values.ToList();
        }
    }
}
