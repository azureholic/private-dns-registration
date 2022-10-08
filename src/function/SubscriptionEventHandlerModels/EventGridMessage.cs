using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionEventHandlerModels;

public class EventGridMessage
{
    public string? Subject { get; set; }
    public string? EventType { get; set; }
    public string? Id { get; set; }
    public Data? Data { get; set; }
    public string? DataVersion { get; set; }
    public string? MetadataVersion { get; set; }
    public DateTime EventTime { get; set; }
    public string? Topic { get; set; }
}

public class Data
{
    //public Authorization? Authorization { get; set; }
    //public Claims? Claims { get; set; }
   // public string? CorrelationId { get; set; }
    public string? ResourceProvider { get; set; }
    public string? ResourceUri { get; set; }
    public string? OperationName { get; set; }
    public string? Status { get; set; }
    public string? SubscriptionId { get; set; }
    public string? TenantId { get; set; }
}

/*
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

*/