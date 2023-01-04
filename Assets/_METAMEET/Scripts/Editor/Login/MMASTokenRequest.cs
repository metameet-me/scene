using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAASTokenRequest 
{
        public string username { get; set; }
        public string password { get; set; }
        public string grant_type { get { return "password"; } }
        public string client_id { get { return "32382cdf-8020-41b9-9a82-f781ce8b45d3"; } }
        public string scope { get { return "https://frussusers.onmicrosoft.com/32382cdf-8020-41b9-9a82-f781ce8b45d3/User.Read openid offline_access"; } }
        public string response_type { get { return "id_token token"; } }

}

public class MAASToken
{
    public string access_token { get; set; }
    public int expires_in { get; set; }
    public string token_type { get; set; }
    public string refresh_token { get; set; }
    public string scope { get; set; }
}
