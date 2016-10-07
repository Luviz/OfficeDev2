using Microsoft.SharePoint.Client;
using Newtonsoft.Json;
using OfficeDevPnP.Core.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSiteSetup {
	public class Setup {
		const string GROUPNAME = "OfficeDev2";
		ClientContext ctx;
		public Setup(ClientContext ctx) {
			this.ctx = ctx;
			//Build Project site
			var web = ProjectSiteBuilder();
			//Create List
			if (web != null)
				ListCreater(web);
		}

		private Web ProjectSiteBuilder() {
			if (!ctx.Web.WebExists("Projects")) {
				ctx.Web.CreateWeb(new SiteEntity {
					Title = "ProjectSite",
					Url = "Projects",
					Lcid = 1033,
					Description = "in this site you can create other porject sites",
					Template = "STS#0"
				});
			}
			return ctx.Site.OpenWeb("Projects");
		}

		private void ListCreater(Web web) {
			List list = null;
			if (!web.ListExists("CreateSubProjects"))
				list = web.CreateList(ListTemplateType.GenericList, "CreateSubProjects", false);
			else
				list = web.GetListByTitle("CreateSubProjects");

			var fields = GetFieldCreationInformationFromJson("CreateSubProjectsFields.json");

			List<Field> fieldsCreated = new List<Field>();

			//Creating the Fields
			fields.ForEach(f => {
				if (!web.FieldExistsById(f.Id))
					fieldsCreated.Add(web.CreateField(f));
			});

			fields.ForEach(f => {
				if (list.Fields.GetById(f.Id) == null)
					list.Fields.Add(web.Fields.GetById(f.Id));
			});

			list.Update();
			web.Context.ExecuteQuery();

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
						Group = sc.Group,
						Required = sc.Required
					});
				});

			}
			return lfci;
		}

		private class SColumns {
			public int Type { get; set; }
			public string DisplayName { get; set; }
			public string Group { get; set; }
			public Guid Guid { get; set; }
			public string InternalName { get; set; }
			public bool Required { get; set; }
		}
	}
}
