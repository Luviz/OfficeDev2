using System;
using System.Security;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using CredentialManagement;

namespace NewsSetup {
    class SPLogin {

        //internal ClientContext context;

        public SPLogin(string webUrl) {
           // context = new ClientContext(webUrl);

            var userCred = new Credential { Target = "luviz.sharepoint.com" };
            if (!userCred.Exists())
                Console.WriteLine("nop");
            userCred.Load();
            Console.WriteLine("bardiajedi@luviz.onmicrosoft.com");
            Console.WriteLine(userCred.Username);

            using (var context = new ClientContext(webUrl)) {
                context.Credentials = new SharePointOnlineCredentials("bardiajedi@luviz.onmicrosoft.com", MakePassSecureAgen(userCred.Password));
                context.Load(context.Web, w => w.Title);
                context.ExecuteQuery();

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Your site title is: " + context.Web.Title);
                //Console.ForegroundColor = defaultForeground;
            }
            
        }

        private SecureString MakePassSecureAgen(string pass) {
            SecureString ret = new SecureString();
            pass.ToCharArray().ToList().ForEach(c => ret.AppendChar(c));
            return ret;

        }
    }
}
