using System.Collections.Generic;

namespace Domainr7.Model
{
    public class DomainrInfo
    {
        public string query { get; set; }
        public string domain { get; set; }
        public string domain_idna { get; set; }
        public string host { get; set; }
        public string subdomain { get; set; }
        public string path { get; set; }
        public string availability { get; set; }
        public Tld tld { get; set; }
        public List<object> subdomains { get; set; }
        public bool subregistration_permitted { get; set; }
        public string www_url { get; set; }
        public string whois_url { get; set; }
        public string register_url { get; set; }
        public List<Registrar> registrars { get; set; }
    }
}
