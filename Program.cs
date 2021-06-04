using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Replikator
{
    class Program
    { 
        static void Main(string[] args)
        {
            string tokenIzvor = Povezi("IzvorBezbednost");
            string tokenOdrediste = Povezi("OdredisteBezbednost");

            ChannelFactory<IBiblioteka> cfIzvor = new ChannelFactory<IBiblioteka>("Izvor");
            ChannelFactory<IBiblioteka> cfOdrediste = new ChannelFactory<IBiblioteka>("Odrediste");

            IBiblioteka kanalIzvor = cfIzvor.CreateChannel();
            IBiblioteka kanalOdrediste = cfOdrediste.CreateChannel();

            while(true)
            {
                Dictionary<long, Clan> clanovi = new Dictionary<long, Clan>();

                try
                {
                    clanovi = kanalIzvor.PreuzmiBazu(tokenIzvor);
                }
                catch (FaultException<BezbednosniIzuzetak> e)
                {
                    Console.WriteLine(e.Detail.Poruka);
                }

                Console.WriteLine("Replicirano {0} podataka", clanovi.Count);

                try
                {
                    kanalOdrediste.PosaljiBazu(tokenOdrediste, clanovi);
                }
                catch (FaultException<BezbednosniIzuzetak> e)
                {
                    Console.WriteLine(e.Detail.Poruka);
                }
                Thread.Sleep(5000);

            }
        }

        static string Povezi(string endpointname)
        {
            ChannelFactory<IBezbednosniMehanizmi> cfB = new ChannelFactory<IBezbednosniMehanizmi>(endpointname);
            IBezbednosniMehanizmi kB = cfB.CreateChannel();

            string token = "";

            try
            {
                token = kB.Autentifikacija("replikator", "repl1kator");
            }
            catch(FaultException<BezbednosniIzuzetak>izuzetak)
            {
                Console.WriteLine(izuzetak.Detail.Poruka);
            }

            return token;
        }
    }
}
