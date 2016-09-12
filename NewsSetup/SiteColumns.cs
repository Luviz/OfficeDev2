using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSetup {
    class SiteColumns {
        public string Guid { get; set; }
        public string DispName { get; set; }
        public string InteName { get; set; }
        public string GroupName { get; set; }

        public FieldType Type { get; set; }
    }
}
