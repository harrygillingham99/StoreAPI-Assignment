using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Cloud.Datastore.V1;

namespace store_api.Objects.Helpers
{
    public static class GoogleKeyHelper
    {
        public static Key ToKey(this long id, DbKinds.DbCollections kind) =>
            new Key().WithElement(kind.GetDescription(), id);
        
        public static long ToId(this Key key) => key.Path.First().Id;
    }
}
