using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionEventHandlers.Model
{

    public class EventGridMessage
    {
        public string subject { get; set; }
        public string eventType { get; set; }
        public string id { get; set; }
        public Data data { get; set; }
        public string dataVersion { get; set; }
        public string metadataVersion { get; set; }
        public DateTime eventTime { get; set; }
        public string topic { get; set; }
    }

    public class Data
    {
        public Authorization authorization { get; set; }
        public Claims claims { get; set; }
        public string correlationId { get; set; }
        public string resourceProvider { get; set; }
        public string resourceUri { get; set; }
        public string operationName { get; set; }
        public string status { get; set; }
        public string subscriptionId { get; set; }
        public string tenantId { get; set; }
    }

    public class Authorization
    {
        public string scope { get; set; }
        public string action { get; set; }
        public Evidence evidence { get; set; }
    }

    public class Evidence
    {
        public string role { get; set; }
    }

    public class Claims
    {
        public string aud { get; set; }
        public string iss { get; set; }
        public string iat { get; set; }
        public string nbf { get; set; }
        public string exp { get; set; }
        public string httpschemasmicrosoftcomclaimsauthnclassreference { get; set; }
        public string aio { get; set; }
        public string httpschemasmicrosoftcomclaimsauthnmethodsreferences { get; set; }
        public string appid { get; set; }
        public string appidacr { get; set; }
        public string httpschemasmicrosoftcom201201devicecontextclaimsidentifier { get; set; }
        public string httpschemasxmlsoaporgws200505identityclaimssurname { get; set; }
        public string httpschemasxmlsoaporgws200505identityclaimsgivenname { get; set; }
        public string groups { get; set; }
        public string ipaddr { get; set; }
        public string name { get; set; }
        public string httpschemasmicrosoftcomidentityclaimsobjectidentifier { get; set; }
        public string onprem_sid { get; set; }
        public string puid { get; set; }
        public string rh { get; set; }
        public string httpschemasmicrosoftcomidentityclaimsscope { get; set; }
        public string httpschemasxmlsoaporgws200505identityclaimsnameidentifier { get; set; }
        public string httpschemasmicrosoftcomidentityclaimstenantid { get; set; }
        public string httpschemasxmlsoaporgws200505identityclaimsname { get; set; }
        public string httpschemasxmlsoaporgws200505identityclaimsupn { get; set; }
        public string uti { get; set; }
        public string ver { get; set; }
        public string xms_tcdt { get; set; }
    }

}
