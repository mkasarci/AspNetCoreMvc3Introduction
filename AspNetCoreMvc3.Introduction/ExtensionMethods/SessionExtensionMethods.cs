using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

namespace AspNetCoreMvc3.Introduction.ExtensionMethods
{
    public static class SessionExtensionMethods
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            string objectString = JsonConvert.SerializeObject(value);
            session.SetString(key, objectString);
        }

        public static T GetObject<T>(this ISession session, string key) where T : class
        {
            return (T) JsonConvert.DeserializeObject<T>(session.GetString(key) ?? throw new ArgumentNullException($"Couldn't find session with key value of: {key}"));
        }
    }
}
