using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core;
using OfficeDevPnP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace NewsSetup {
	public class Installer {
		const string GROUPNAME = "OfficeDev2";
		public Installer(ClientContext ctx) {
			Console.WriteLine("NewsSetup ...");
			//Build ct 
			var newsCT = BuildContentType(ctx);
			//Build site Columns
			BuildSiteColumns(ctx, "SiteColumns.json");
			//connect sc to ct
			ConntectSiteCloumnsToCT(ctx, newsCT);
			Console.WriteLine("NewsSetup DONE!!");
		}

		internal ContentType BuildContentType(ClientContext ctx) {
			if (!ctx.Web.ContentTypeExistsById(Guids.ContentTypes.GetNews_CT_Guid())) {
				Console.Write("Creataing ContentType ...");
				ContentTypeCreationInformation ctci = new ContentTypeCreationInformation();
				ctci.Name = "OD2_ct_NewsPage";
				ctci.Group = GROUPNAME;
				ctci.Id = Guids.ContentTypes.GetNews_CT_Guid();
				ctci.Description = "News Page for Office Dev 2 Assinment";

				var NewsPages = ctx.Web.ContentTypes.Add(ctci);
				ctx.Web.Update();
				ctx.ExecuteQuery();
				Console.WriteLine("DONE!");
				return NewsPages;
			}
			else {
				Console.WriteLine("ContentTpye Exists");
				return ctx.Web.GetContentTypeById(Guids.ContentTypes.GetNews_CT_Guid());
			}
		}

		private void BuildSiteColumns(ClientContext ctx, string pathOfJson) {
			Console.Write("Working on SiteColumns ...");
			var Fields = GetFieldCreationInformationFromJson(pathOfJson);
			Fields.ForEach(f => {
				if (!ctx.Web.FieldExistsById(f.Id))
					ctx.Web.CreateField(f);
			});
			Console.WriteLine("DONE!");
		}

		private void ConntectSiteCloumnsToCT(ClientContext ctx, ContentType ct) {
			Console.Write("Conecting SiteColumns to ContentType...");
			ct.AddFieldById(Guids.SiteColumns.INGRESS);
			ct.AddFieldById(Guids.SiteColumns.IMG);
			ct.AddFieldById(Guids.SiteColumns.AUTHOR);
			ct.AddFieldById(Guids.SiteColumns.CONTENT);
			ct.AddFieldById(Guids.SiteColumns.ARTICLE_DATE);
			Console.WriteLine("DONE!");
			ct.Update(true);
			ctx.ExecuteQuery();
		}

		private static List<FieldCreationInformation> GetFieldCreationInformationFromJson(string path) {
			List<FieldCreationInformation> lfci;
			using (StreamReader sr = new StreamReader(path)) {
				string json = sr.ReadToEnd();
				List<SColumns> scs = JsonConvert.DeserializeObject<List<SColumns>>(json);
				lfci = new List<FieldCreationInformation>();
				scs.ForEach(sc => {
					lfci.Add(new FieldCreationInformation((FieldType)sc.Type) {
						DisplayName = sc.DisplayName,
						InternalName = sc.InternalName,
						Id = sc.Guid,
						Group = sc.Group
					});
				});
			}
			return lfci;
		}

		private class SColumns {
			public Guid Guid { get; set; }
			public string DisplayName { get; set; }
			public string InternalName { get; set; }
			public int Type { get; set; }
			public string Group { get; set; }
		}
	}
}
