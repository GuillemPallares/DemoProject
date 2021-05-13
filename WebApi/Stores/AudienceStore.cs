using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using WebApi.Models;

namespace WebApi.Stores
{
    /// <summary>
    /// Create an in memmory Audience store so we can have a Resourceserver in the API to create JWT Tokens.
    /// </summary>
    public static class AudiencesStore
    {
        public static ConcurrentDictionary<string, Audience> AudiencesList = new ConcurrentDictionary<string, Audience>();

        static AudiencesStore()
        {
            AudiencesList.TryAdd(ConfigurationManager.AppSettings["Audience"],
                                new Audience
                                {
                                    ClientId = ConfigurationManager.AppSettings["Audience"],
                                    Base64Secret = ConfigurationManager.AppSettings["Secret"],
                                    Name = "ResourceServer.Api 1"
                                });
        }
        // We leaved commnet for the demo, no need of adding audiences.
        // public static Audience AddAudience(string name)
        // {
        //     var clientId = Guid.NewGuid().ToString("N");
        // 
        //     var key = new byte[32];
        //     RNGCryptoServiceProvider.Create().GetBytes(key);
        //     var base64Secret = TextEncodings.Base64Url.Encode(key);
        // 
        //     Audience newAudience = new Audience { ClientId = clientId, Base64Secret = base64Secret, Name = name };
        //     AudiencesList.TryAdd(clientId, newAudience);
        //     return newAudience;
        // }

        public static Audience FindAudience(string clientId)
        {
            Audience audience = null;
            if (AudiencesList.TryGetValue(clientId, out audience))
            {
                return audience;
            }
            return null;
        }
    }
}