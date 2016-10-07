using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSiteBuilderWithSBQueue {
	class SubProjectAddOn {
		private Web Web;

		public SubProjectAddOn(Web web) {
			Web = web;

			//Special Documents
			Web.CreateDocumentLibrary("Special Library");

			//Important Links
			Web.CreateList(ListTemplateType.Links, "Important Links", false);
			//CreateLinkList();
		}


		
		private void CreateLinkList() {
			ListCreationInformation lci = new ListCreationInformation() {
				Description = "Broh There are IMPORTANT links here!!",
				Title = "Important Links",
				TemplateType = (int)ListTemplateType.Links,
			};
			List impLinks = Web.Lists.Add(lci);

			Web.Context.Load(impLinks);
			Web.Context.ExecuteQuery();
		}
	}
}
