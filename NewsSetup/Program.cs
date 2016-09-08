using CredentialManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSetup {
    class Program {
        static void Main(string[] args) {
            SPLogin spl = new SPLogin("https://luviz.sharepoint.com/SitePages/DevHome.aspx");

            Console.WriteLine("All done!");
            Console.ReadLine();
        }
    }
}
