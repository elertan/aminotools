using System;
using System.Collections.Generic;
using AminoApi.Models.User;

namespace AminoApi.Models.Auth
{
    /*
     {
    "account": {
        "status": 0,
        "uid": "abbaed9b-e609-4e17-90d0-8a53d67e82e9",
        "phoneNumberActivation": 0,
        "emailActivation": 1,
        "facebookID": null,
        "advancedSettings": {
            "amplitudeAnalyticsEnabled": 0,
            "amplitudeAppId": null
        },
        "mediaList": [
            [
                100,
                "http://pm1.narvii.com/6525/80712d565bc453fba3f436340a0416521bd84a38_00.jpg",
                null
            ]
        ],
        "dateOfBirth": null,
        "role": 0,
        "latitude": null,
        "phoneNumber": null,
        "email": "denkievits@gmail.com",
        "username": null,
        "modifiedTime": "2017-07-15T12:31:12Z",
        "twitterID": null,
        "activation": 1,
        "address": null,
        "nickname": "Runic Slash",
        "googleID": null,
        "icon": "http://pm1.narvii.com/6525/fb1c05c417593cc9e4df405719034208c6a11892_00.jpg",
        "gender": null,
        "longitude": null,
        "extensions": {
            "deviceInfo": {
                "lastClientType": 100
            },
            "leftNdcIds": [
                48180018
            ]
        },
        "createdTime": "2017-07-10T21:02:28Z"
    },
    "secret": "31 kWfnbSUJ abbaed9b-e609-4e17-90d0-8a53d67e82e9 178.85.19.253 d5ed5be317f883719728b66e5d9d7a21bf359605 1 1503237875 PmCpoElDCrXcEVgTqJp-4rX65Gk",
    "api:message": "OK",
    "sid": "AYACfXEBKEsASwFLAU5LAooR6YJ-1lOK0JAXTgnmm-26qwBLA0sASwRVBLJVE_1LBUrzlplZSwZLZHUuT1mIgf9-cAUFNbGYR7tbySZwPEQ",
    "api:statuscode": 0,
    "api:duration": "0.026s",
    "api:timestamp": "2017-08-20T14:04:35Z"
}
         */
    public class Account : UserProfile
    {
        public string Secret { get; set; }
        public string Sid { get; set; }
        public bool PhoneNumberActivation { get; set; }
        public bool EmailActivation { get; set; }
        public string FacebookId { get; set; }
        public AdvancedSettings AdvancedSettings { get; set; }
        public string Email { get; set; }

        public override void JsonResolve(Dictionary<string, object> data)
        {
            Secret = data.Resolve<string>("secret");
            Sid = data.Resolve<string>("sid");

            var account = data["account"].ToJObject().ToDictionary();

            base.JsonResolve(account);
            Email = account.Resolve<string>("email");

            AdvancedSettings = new AdvancedSettings();
            AdvancedSettings.JsonResolve(account["advancedSettings"].ToJObject().ToDictionary());
        }
    }
}
