using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IBiblioteka> cf = new ChannelFactory<IBiblioteka>("ServisBiblioteke");
            IBiblioteka kanal = cf.CreateChannel();
            Console.WriteLine("Uspjesno povezan na port 4000");

            ChannelFactory<IBezbednosniMehanizmi> cfBezbednost = new ChannelFactory<IBezbednosniMehanizmi>("ServisBezbednost");
            IBezbednosniMehanizmi kanalBezbednost = cfBezbednost.CreateChannel();
            Console.WriteLine("Uspjesno povezan na port 4000 Bezbednosti");

            string tokenP = "";
            string tokenA = "";

            try
            {
                tokenP = kanalBezbednost.Autentifikacija("pera", "p3ra");
            }
            catch(FaultException<BezbednosniIzuzetak> izuzetak)
            {
                Console.WriteLine(izuzetak.Detail.Poruka);
            }

            try
            {
                tokenA = kanalBezbednost.Autentifikacija("admin", "adm1n");
            }
            catch (FaultException<BezbednosniIzuzetak> izuzetak)
            {
                Console.WriteLine(izuzetak.Detail.Poruka);
            }

            try
            {
                kanal.DodajClana(tokenA, new Clan("Dejan", "Kurdulija", 123456789));
                Console.WriteLine("Uspjesno dodat clan");
            }
            catch(FaultException<BezbednosniIzuzetak>izuzetak)
            {
                Console.WriteLine(izuzetak.Detail.Poruka);
            }

            try
            {
                kanal.DodajClana(tokenP, new Clan("Zdravko", "Milinkovic", 987654321));
                Console.WriteLine("Uspjesno dodat clan");
            }
            catch (FaultException<BezbednosniIzuzetak> izuzetak)
            {
                Console.WriteLine(izuzetak.Detail.Poruka);
            }

            Console.ReadKey();

        }
    }
}
