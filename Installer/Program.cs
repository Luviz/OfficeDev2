﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Entities;
using NewsSetup;
using ProjectSiteSetup;

namespace Installer {
	class Program {
		static void Main(string[] args) {
			using (ClientContext ctx = GetAppOnlyCtx(new Uri("https://luviz.sharepoint.com/sites/OfficeDev2"))) {
				//run New Setup
				//new NewsSetup.Installer(ctx);
				//Setup ProjectSite
				Console.Write("Setting up Project site and Createing the List for subSiteCreation ... ");
				new Setup(ctx);
				Console.WriteLine("Done!");

				Console.WriteLine("All Ready to go!");
				Console.WriteLine("Press ENTER to close the window");
			}
			Console.ReadLine();
		}

		public static ClientContext GetAppOnlyCtx(Uri siteUri) {
			string realm = TokenHelper.GetRealmFromTargetUrl(siteUri);
			var token = TokenHelper.GetAppOnlyAccessToken(TokenHelper.SharePointPrincipal, siteUri.Authority, realm).AccessToken;
			return TokenHelper.GetClientContextWithAccessToken(siteUri.ToString(), token);
		}
	}
}
