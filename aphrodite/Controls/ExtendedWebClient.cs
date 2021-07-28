using System;
using System.Net;

namespace aphrodite.Controls {
    public class ExtendedWebClient : WebClient {
        public string Method { get; set; }
        public CookieContainer CookieContainer { get; set; }

        public ExtendedWebClient() {
            CookieContainer = new CookieContainer();
        }
        public ExtendedWebClient(CookieContainer Cookies) {
            CookieContainer = Cookies;
        }

        protected override WebRequest GetWebRequest(Uri address) {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);

            request.CookieContainer = this.CookieContainer;
            if (!string.IsNullOrEmpty(Method)) {
                request.Method = Method;
            }

            return request;
        }
    }
}
