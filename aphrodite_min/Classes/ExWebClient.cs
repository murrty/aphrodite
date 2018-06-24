﻿using System;
using System.Net;

namespace aphrodite_min {
    public class ExWebClient : WebClient {
        public string Method { get; set; }

        protected override WebRequest GetWebRequest(Uri address) {
            WebRequest webRequest = base.GetWebRequest(address);

            if (!string.IsNullOrEmpty(Method))
                webRequest.Method = Method;

            return webRequest;
        }
    }
}
