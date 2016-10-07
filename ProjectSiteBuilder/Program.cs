using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeDevPnP.Core.Entities;
using OfficeDevPnP.Core.Enums;
using Microsoft.SharePoint.Client;
//add pnp

namespace ProjectSiteBuilder {
	class Program {
		static void Main(string[] args) {
			using (ClientContext ctx = GetAppOnlyCtx(new Uri("https://luviz.sharepoint.com/sites/OfficeDev2"))) {
				
			}
		}

		public static ClientContext GetAppOnlyCtx(Uri siteUri) {
			string realm = TokenHelper.GetRealmFromTargetUrl(siteUri);
			var token = TokenHelper.GetAppOnlyAccessToken(TokenHelper.SharePointPrincipal, siteUri.Authority, realm).AccessToken;
			return TokenHelper.GetClientContextWithAccessToken(siteUri.ToString(), token);
		}
	}
}
