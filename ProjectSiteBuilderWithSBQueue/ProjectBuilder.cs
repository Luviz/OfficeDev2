using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSiteBuilderWithSBQueue {

	/*	Props
		{
			"Title": [From SP list],
			"LcID": [From SP list],
			"Description": [From SP list],
			"RealtiveUrl": [From SP list],
			"Url":"[Constant the root location of project Sites]" atm set as "https://luviz.sharepoint.com/sites/OfficeDev2/Projects"
			"CreatedByEmail" : "email of the item creater"
		}
	 */

	class ProjectBuilder {
		private ClientContext Ctx;
		private Dictionary<string, object> Props;


		public ProjectBuilder(IDictionary<string, object> props) {
			GetAppOnlyCtx(new Uri(props["Url"] as string));
			Props = new Dictionary<string, object>(props);
			CreateSubSites();
		}

		private void CreateSubSites() {
			if (!Ctx.Web.WebExists($"{Props["RealtiveUrl"] as string}")) {
				var web = Ctx.Web.CreateWeb(new SiteEntity {
					Title = Props["Title"] as string,
					Url = Props["RealtiveUrl"] as string,
					Lcid = uint.Parse(Props["LcID"] as string),
					Description = Props["Description"] as string,
					Template = "STS#0" //constant set on teamsite but can easly be chanaged
				});
				new SubProjectAddOn(web);
				//this fucks the web for what ever resone Aske David WTF am I dong WORNG!!!
				//web.CreateDefaultAssociatedGroups($"i:0#.f|membership|{Props["CreatedByEmail"] as string}", "i:0#.f|membership|bardiajedi@luviz.onmicrosoft.com", $"{Props["RealtiveUrl"] as string}-AdminGroup");
			}
			else
				throw new Exception("Site url in use");
		}

		public void GetAppOnlyCtx(Uri siteUri) {
			string realm = TokenHelper.GetRealmFromTargetUrl(siteUri);
			var token = TokenHelper.GetAppOnlyAccessToken(TokenHelper.SharePointPrincipal, siteUri.Authority, realm).AccessToken;
			Ctx = TokenHelper.GetClientContextWithAccessToken(siteUri.ToString(), token);
		}
	}
}
