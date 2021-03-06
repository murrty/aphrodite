﻿using System;
using System.Net;

namespace aphrodite.Controls {
    public class ExtendedWebClient : WebClient {
        public string Method { get; set; }

        protected override WebRequest GetWebRequest(Uri address) {
            WebRequest webRequest = base.GetWebRequest(address);

            if (!string.IsNullOrEmpty(Method))
                webRequest.Method = Method;
            
            return webRequest;
        }
    }
}
