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
using Microsoft.SharePoint.Client.Taxonomy;
using Newtonsoft.Json.Converters;

namespace NewsSetup {
	public class Installer {
		const string GROUPNAME = "OfficeDev2";
		private static ClientContext Ctx;

		public Installer(ClientContext ctx) {
			Ctx = ctx;
			Console.WriteLine("NewsSetup ...");
			//Build Taxonomy
			BuildTaxonomy();
			//Build ct 
			var newsCT = BuildContentType();
			//Build site Columns
			BuildSiteColumns("SiteColumns.json");
			//connect sc to ct
			ConntectSiteCloumnsToCT(newsCT);
			Console.WriteLine("NewsSetup DONE!!");
		}

		internal ContentType BuildContentType() {
			if (!Ctx.Web.ContentTypeExistsById(Guids.ContentTypes.GetNews_CT_Guid())) {
				Console.Write("Creataing ContentType ...");
				ContentTypeCreationInformation ctci = new ContentTypeCreationInformation();
				ctci.Name = "OD2_ct_NewsPage";
				ctci.Group = GROUPNAME;
				ctci.Id = Guids.ContentTypes.GetNews_CT_Guid();
				ctci.Description = "News Page for Office Dev 2 Assinment";

				var NewsPages = Ctx.Web.ContentTypes.Add(ctci);
				Ctx.Web.Update();
				Ctx.ExecuteQuery();
				Console.WriteLine("DONE!");
				return NewsPages;
			}
			else {
				Console.WriteLine("ContentTpye Exists");
				return Ctx.Web.GetContentTypeById(Guids.ContentTypes.GetNews_CT_Guid());
			}
		}

		internal void BuildTaxonomy() {
			Console.Write("Building Taxonomy ...");
			var session = Ctx.Site.GetTaxonomySession();
			Ctx.Load(session);
			Ctx.ExecuteQuery();
			if (session != null) {
				TermStore termStore = session.GetDefaultSiteCollectionTermStore();
				if (termStore != null) {

					if (termStore.GetTermSet(Guids.Taxonomy.DEPARTMANT_TERMSET.ToGuid()) == null) {
						TermGroup myGroup = termStore.CreateGroup(GROUPNAME, Guid.NewGuid());
						TermSet myTermSet = myGroup.CreateTermSet("Department", Guids.Taxonomy.DEPARTMANT_TERMSET.ToGuid(), 1033);
						myTermSet.CreateTerm("HR", 1033, Guid.NewGuid());
						myTermSet.CreateTerm("Economy", 1033, Guid.NewGuid());
						myTermSet.CreateTerm("SharePoint", 1033, Guid.NewGuid());

						Ctx.ExecuteQuery();
						Console.WriteLine("DONE!");
					}
					else {
						Console.WriteLine("TermStore Departmant Exsists!");
					}
				}
			}

		}

		private void BuildSiteColumns(string pathOfJson) {
			Console.Write("Working on SiteColumns ...");
			var fields = GetFieldCreationInformationFromJson(pathOfJson);
			fields.ForEach(f => {
				if (f.GetType() == typeof(TaxonomyFieldCreationInformation)) {
					if (!Ctx.Web.FieldExistsById(f.Id))
						Ctx.Web.CreateTaxonomyField(f as TaxonomyFieldCreationInformation);
				}
				else {
					if (!Ctx.Web.FieldExistsById(f.Id))
						Ctx.Web.CreateField(f);
				}

			});


			//Creating the Taxonomy Feilds
			Console.WriteLine("DONE!");
		}

		private void ConntectSiteCloumnsToCT(ContentType ct) {
			Console.Write("Conecting SiteColumns to ContentType...");
			ct.AddFieldById(Guids.SiteColumns.INGRESS);
			ct.AddFieldById(Guids.SiteColumns.IMG);
			ct.AddFieldById(Guids.SiteColumns.AUTHOR);
			ct.AddFieldById(Guids.SiteColumns.CONTENT);
			ct.AddFieldById(Guids.SiteColumns.ARTICLE_DATE);
			ct.AddFieldById(Guids.SiteColumns.DEPARTMANT);
			Console.WriteLine("DONE!");
			ct.Update(true);
			Ctx.ExecuteQuery();


		}

		private static List<FieldCreationInformation> GetFieldCreationInformationFromJson(string path) {

			List<FieldCreationInformation> lfci;
			using (StreamReader sr = new StreamReader(path)) {
				string json = sr.ReadToEnd();
				List<SColumns> scs = JsonConvert.DeserializeObject<List<SColumns>>(json);
				lfci = new List<FieldCreationInformation>();
				scs.ForEach(sc => {
					if (!sc.IsTax) {
						lfci.Add(new FieldCreationInformation((FieldType)sc.Type) {
							DisplayName = sc.DisplayName,
							InternalName = sc.InternalName,
							Id = sc.Guid,
							Group = sc.Group
						});
					}
					else {  // Taxonomy Site Columns
						lfci.Add(new TaxonomyFieldCreationInformation {
							DisplayName = sc.DisplayName,
							InternalName = sc.InternalName,
							Id = sc.Guid,
							Group = sc.Group,
							TaxonomyItem = GetTermSetById(sc.TermSetID),
							MultiValue = sc.MultiValue
						});
					}
				});

			}
			return lfci;
		}

		private static TaxonomyItem GetTermSetById(Guid termSetID) {
			var session = Ctx.Site.GetTaxonomySession();
			TermSet ret = null;
			TaxonomySession taxonomySession = TaxonomySession.GetTaxonomySession(Ctx.Web.Context);
			TermStore termStore = taxonomySession.GetDefaultSiteCollectionTermStore();
			TermGroup termGroup = termStore.Groups.GetByName(GROUPNAME);
			TermSet termSet = termGroup.TermSets.GetById(termSetID);
			Ctx.Web.Context.Load(termStore);
			Ctx.Web.Context.Load(termSet);
			Ctx.Web.Context.ExecuteQueryRetry();
			ret = termSet;
			return ret;
		}

		private class SColumns {
			public Guid Guid { get; set; }
			public string DisplayName { get; set; }
			public string InternalName { get; set; }
			public int Type { get; set; }
			public string Group { get; set; }
			public Guid TermSetID { get; set; }
			public bool MultiValue { get; set; }
			public bool IsTax { get; set; }
		}
	}
}
