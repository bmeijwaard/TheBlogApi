# The Blog Api
### !! Currently in progress !!

*Open source* and *free to use*, **blog and photography** REST API in .NET Core 2.1


### The user secrets *format*
```` JSON
{
  "ConnectionStrings": {
    "DefaultConnection": "string",
    "AzureWebJobsDashboard": "string",
    "AzureWebJobsStorage": "string"
  },
  "StorageSettings": {
    "ConnectionString": "string"
  },
  "CahceSettings": {
    "ConnectionString": "string",
    "UseCache": true,
    "DefaultExpiry": "00:01"
  },
  "SmtpSettings": {
    "SendGridUser": "string",
    "SendGridPassword": "string",
    "FromEmailAddress": "string (email)",
    "Smtp": "string",
    "SmtpPort": "string (int)"
  },
  "ExceptionSettings": {
    "EmailAddresses": [
      "string (email)"
    ],
    "Env": "string"
  },
  "JWTSettings": {
    "SiteAddress": "Uri",
    "Audience": "string",
    "SecurityKey": "string",
    "DeviceKey": "guid",
    "ExpiresAfterHours": int
  },
  "ApplicationInsights": {
    "InstrumentationKey": "guid"
  },
  "EncryptionSettings": {
    "Password": "string",
    "PrivatePassword": "string",
    "BlockSize": int,
    "KeySize": int,
    "Salt": [ int ]
  },
  "WebjobSettings": {
    "Username": "string",
    "Password": "string",
    "Webhook": "Uri"
  }
}
````