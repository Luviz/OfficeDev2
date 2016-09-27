using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSetup {
	internal class Guids {
		public class ContentTypes {
			public const string ARTICLE_PAGE_CT = "0x010100C568DB52D9D0A14D9B2FDCC96666E9F2007948130EC3DB064584E219954237AF3900242457EFB8B24247815D688C526CD44D";
			public const string NEWS_PAGE_CT = "97EDD7BDC81B4A279B21B0AF11EBF114";

			public static string GetNews_CT_Guid() { return ARTICLE_PAGE_CT + "00" + NEWS_PAGE_CT; }
		}
		public class SiteColumns {
			//site columns
			//Title - inherted
			//Ingress
			public const string INGRESS = "{F529FF7D-6DE9-4DA6-BEB9-741FB80BD95C}";
			//Image Publishing Image
			public const string IMG = "{77E37F68-D329-4879-9706-2D41157442B4}";
			//Author
			public const string AUTHOR = "{A3937500-F5D9-454D-AF81-6F880DD97EF0}";
			//Content publishing html
			public const string CONTENT = "{B67DF1CC-08A7-4CF6-AE5F-3E835F8E2D8E}";
			//article Date
			public const string ARTICLE_DATE = "{43340732-FF49-4E96-88D4-89D78177DDC1}";
			//Depatment Taxomy
			public const string DEPARTMANT = "{48B1C174-F857-464F-8969-D99AF32DA236}";
			//Keyword Taxonomy
			public const string KEYWORDS = "{7155209B-B7CB-47F7-8A8E-22BDCA9ED5BB}";
		}

		public class Taxonomy {
			//Departmant TS
			public const string DEPARTMANT_TERMSET = "{F04CD70B-FDC5-4054-A034-106ADA653F20}";
		}
	}
}
