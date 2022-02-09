using System;
using System.ComponentModel;
using System.Net;

namespace murrty.classcontrols {

    /// <summary>
    /// The method that the connection should use.
    /// </summary>
    public enum HttpMethod {
        /// <summary>
        /// Starts two-way communications with the requested resource.
        /// </summary>
        CONNECT,
        /// <summary>
        /// Deletes the specified resource.
        /// </summary>
        DELETE,
        /// <summary>
        /// Requests a representation of the specified resource.
        /// </summary>
        GET,
        /// <summary>
        /// Requests the headers that would be returned if the requests' URL was instead requested with GET.
        /// </summary>
        HEAD,
        /// <summary>
        /// Requests permitted communication options for the URL or server.
        /// </summary>
        OPTIONS,
        /// <summary>
        /// Applies partial modifications to a resource.
        /// </summary>
        PATCH,
        /// <summary>
        /// Sends data to the server as an addition to the target.
        /// </summary>
        POST,
        /// <summary>
        /// Sends data to the server as a replacement of the target.
        /// </summary>
        PUT,
        /// <summary>
        /// Performs a message loop-back test along with the path to the target resource, used for debugging.
        /// </summary>
        TRACE
    }

    /// <summary>
    /// An extended control to include extra usability to the base WebClient class.
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    [ToolboxItem(false)]
    public class ExtendedWebClient : WebClient {

        /// <summary>
        /// Gets or sets the CookieContainer used to maintain the cookies when accessing online resources.
        /// </summary>
        public CookieContainer CookieContainer { get; set; } = null;

        /// <summary>
        /// Sets the WebClient METHOD when connecting to the online resource.
        /// <para>Default value is GET.</para>
        /// </summary>
        public HttpMethod Method { get; set; } = HttpMethod.GET;

        /// <summary>
        /// Gets or sets the UserAgent of the WebClient.
        /// </summary>
        public string UserAgent { get; set; } = null;

        /// <summary>
        /// An extended control to include extra usability to the base WebClient class.
        /// </summary>
        public ExtendedWebClient() {
            this.CookieContainer = new();
        }

        /// <summary>
        /// An extended control to include extra usability to the base WebClient class.
        /// </summary>
        /// <param name="CookieContainer">The <seealso cref="System.Net.CookieContainer"/> containing requested cookies that the user wants to use.</param>
        public ExtendedWebClient(CookieContainer CookieContainer) {
            this.CookieContainer = CookieContainer;
        }

        [System.Diagnostics.DebuggerHidden]
        protected override WebRequest GetWebRequest(Uri address) =>
            ManageRequest(base.GetWebRequest(address));

        /// <summary>
        /// Manages the <see cref="WebRequest"/> before contacting the online resource.
        /// </summary>
        /// <param name="Request">The <see cref="WebRequest"/> that will be modified to include the extended functionality.</param>
        /// <returns>The modified WebRequest conforming to the extensions.</returns>
        private WebRequest ManageRequest(WebRequest Request) {
            // Check the Method property. Use "GET" if the property is null, empty, or whitespace.
            Request.Method = this.Method.ToString();

            // We're gonna cast it as a HttpWebRequest to add Cookies as well as the IfModifiedSince header.
            if (Request is HttpWebRequest RequestAsHttp) {
                // Set the UserAgent.
                RequestAsHttp.UserAgent = this.UserAgent;

                // Set the CookieContainer.
                RequestAsHttp.CookieContainer = this.CookieContainer;

            }

            // Finally, return the modified request.
            return Request;
        }
    }
}