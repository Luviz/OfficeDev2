using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core;
using OfficeDevPnP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSetup {
    public class Installer {

        const string GROUPNAME = "OfficeDev2";

        public Installer(ClientContext ctx) {
            //Build ct 
            //Build site Columns
            //connect sc to ct
        }

        internal ContentType BuildContentType(ClientContext ctx) {
            if (!ctx.Web.ContentTypeExistsById(Guids.ContentTypes.GetNews_CT_Guid())) {
                ContentTypeCreationInformation ctci = new ContentTypeCreationInformation();
                ctci.Name = "OD2_ct_NewsPage";
                ctci.Group = GROUPNAME;
                ctci.Id = Guids.ContentTypes.GetNews_CT_Guid();
                ctci.Description = "News Page for Office Dev 2 Assinment";

                var NewsPages = ctx.Web.ContentTypes.Add(ctci);

                return NewsPages;
            }
            else {
                return ctx.Web.GetContentTypeById(Guids.ContentTypes.GetNews_CT_Guid());
            }
        }

        //setup site Columns 
        private List<SiteColumns> SetupSiteColmns() {
            List<SiteColumns> ret = new List<SiteColumns>();
            //Ingrees
            ret.Add(new SiteColumns {
                DispName = "Ingress",
                InteName = "ingress",
                GroupName = GROUPNAME,
                Type = FieldType.Text,
                Guid = Guids.SiteColumns.INGRESS
            });
            //Image 
            ret.Add(new SiteColumns {
                DispName = "Publishing Image",
                InteName = "pubimg",
                GroupName = GROUPNAME,
                Type = FieldType.URL,
                Guid = Guids.SiteColumns.IMG
            });
            //Author
            ret.Add(new SiteColumns {
                DispName = "Author",
                InteName = "author",
                GroupName = GROUPNAME,
                Type = FieldType.Text,
                Guid = Guids.SiteColumns.AUTHOR
            });
            //Content
            ret.Add(new SiteColumns {
                DispName = "Content",
                InteName = "conten",
                GroupName = GROUPNAME,
                Type = FieldType.Text, //have to be RichText
                Guid = Guids.SiteColumns.CONTENT
            });
            //Aricle Date
            ret.Add(new SiteColumns {
                DispName = "Aritcle Date",
                InteName = "ariticledate",
                GroupName = GROUPNAME,
                Type = FieldType.DateTime,
                Guid = Guids.SiteColumns.ARTICLE_DATE
            });

            return ret;
        }


    }
}
