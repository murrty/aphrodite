﻿using System;
using System.Net;

namespace aphrodite {

    // FINISH IMPLEMENTING RETRY WHEN REPORTING ERRORS

    class ErrorLog {
        /// <summary>
        /// Reports any web errors that are caught.
        /// </summary>
        /// <param name="WebException">The WebException that was caught</param>
        /// <param name="WebsiteAddress">The URL that (might-have) caused the problem</param>
        /// <param name="ErrorClass">The class that the WebException occured in</param>
        public static void ReportWebException(WebException WebException, string WebsiteAddress, string ErrorClass) {
            string OutputFile = string.Empty;
            string CustomDescriptionBuffer = string.Empty;
            bool UseCustomDescription = false;

            using (frmException ExceptionDisplay = new frmException()) {
                ExceptionDisplay.ReportedWebException = WebException;
                ExceptionDisplay.FromLanguage = false;

                if ((int)WebException.Status == 418) {
                    UseCustomDescription = true;
                    CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                    "\r\n\r\nCannot brew coffee" +
                                    "\r\nI'm a teapot";
                }
                else {
                    switch (WebException.Status) {
                        #region NameResolutionFailure
                        case WebExceptionStatus.NameResolutionFailure:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nName resolution failure" +
                                            "\r\nThe name resolver service could not resolve the host name.";
                            break;
                        #endregion
                        #region ConnectFailure
                        case WebExceptionStatus.ConnectFailure:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nConnection failure" +
                                            "\r\nThe remote service point could not be contacted at the transport level.";
                            break;
                        #endregion
                        #region RecieveFailure
                        case WebExceptionStatus.ReceiveFailure:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nRecieve failure" +
                                            "\r\nA complete response was not received from the remote server.";
                            break;
                        #endregion
                        #region SendFailure
                        case WebExceptionStatus.SendFailure:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nSend failure" +
                                            "\r\nA complete response could not be sent to the remote server.";
                            break;
                        #endregion
                        #region PipelineFailure
                        case WebExceptionStatus.PipelineFailure:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nPipeline failure" +
                                            "\r\nThe request was a piplined request and the connection was closed before the response was received.";
                            break;
                        #endregion
                        #region RequestCanceled
                        case WebExceptionStatus.RequestCanceled:
                            return;
                        #endregion
                        #region ProtocolError
                        case WebExceptionStatus.ProtocolError:
                            var WebResponse = WebException.Response as HttpWebResponse;

                            if (WebResponse != null) {
                                UseCustomDescription = true;
                                switch ((int)WebResponse.StatusCode) {
                                    #region StatusCodes
                                    #region default / unspecified
                                    default:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned " + WebResponse.StatusCode.ToString() +
                                            "\r\n" + WebResponse.StatusDescription.ToString();
                                        break;
                                    #endregion

                                    #region 301 Moved / Moved permanently
                                    case 301:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 301 - Moved / Moved permanently" +
                                            "\r\nThe requested information has been moved to the URI specified in the Location header.";
                                        break;
                                    #endregion

                                    #region 400 Bad request
                                    case 400:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 400 - Bad request" +
                                            "\r\nThe request could not be understood by the server.";
                                        break;
                                    #endregion

                                    #region 401 Unauthorized
                                    case 401:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 401 - Unauthorized" +
                                            "\r\nThe requested resource requires authentication.";
                                        break;
                                    #endregion

                                    #region 402 Payment required
                                    case 402:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 402 - Payment required" +
                                            "\r\nPayment is required to view this content.\r\nThis status code isn't natively used.";
                                        break;
                                    #endregion

                                    #region 403 Forbidden
                                    case 403:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 403 - Forbidden" +
                                            "\r\nYou do not have permission to view this file.";
                                        break;
                                    #endregion

                                    #region 404 Not found
                                    case 404:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 404 - Not found" +
                                            "\r\nThe file does not exist on the server.";
                                        break;
                                    #endregion

                                    #region 405 Method not allowed
                                    case 405:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 405 - Method not allowed" +
                                            "\r\nThe request method (GET) is not allowed on the requested resource.";
                                        break;
                                    #endregion

                                    #region 406 Not acceptable
                                    case 406:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 406 - Not acceptable" +
                                            "\r\nThe client has indicated with Accept headers that it will not accept any of the available representations from the resource.";
                                        break;
                                    #endregion

                                    #region 407 Proxy authentication required
                                    case 407:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 407 - Proxy authentication required" +
                                            "\r\nThe requested proxy requires authentication.";
                                        break;
                                    #endregion

                                    #region 408 Request timeout
                                    case 408:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 408 - Request timeout" +
                                            "\r\nThe client did not send a request within the time the server was expection the request.";
                                        break;
                                    #endregion

                                    #region 409 Conflict
                                    case 409:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 409 - Conflict" +
                                            "\r\nThe request could not be carried out because of a conflict on the server.";
                                        break;
                                    #endregion

                                    #region 410 Gone
                                    case 410:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 410 - Gone" +
                                            "\r\nThe requested resource is no longer available.";
                                        break;
                                    #endregion

                                    #region 411 Length required
                                    case 411:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 410 - Length required" +
                                            "\r\nThe required Content-length header is missing.";
                                        break;
                                    #endregion

                                    #region 412 Precondition failed
                                    case 412:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 412 - Precondition failed" +
                                            "\r\nA condition set for this request failed, and the request cannot be carried out.";
                                        break;
                                    #endregion

                                    #region 413 Request entity too large
                                    case 413:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 413 - Request entity too large" +
                                            "\r\nThe request is too large for the server to process.";
                                        break;
                                    #endregion

                                    #region 414 Request uri too long
                                    case 414:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 414 - Request uri too long" +
                                            "\r\nThe uri is too long.";
                                        break;
                                    #endregion

                                    #region 415 Unsupported media type
                                    case 415:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 415 - Unsupported media type" +
                                            "\r\nThe request is an unsupported type.";
                                        break;
                                    #endregion

                                    #region 416 Requested range not satisfiable
                                    case 416:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 416 - Requested range not satisfiable" +
                                            "\r\nThe range of data requested from the resource cannot be returned.";
                                        break;
                                    #endregion

                                    #region 417 Expectation failed
                                    case 417:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 417 - Expectation failed" +
                                            "\r\nAn expectation given in an Expect header could not be met by the server.";
                                        break;
                                    #endregion

                                    #region 426 Upgrade required
                                    case 426:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 426 - Upgrade required" +
                                            "\r\nNo information is available about this error code.";
                                        break;
                                    #endregion

                                    #region 500 Internal server error
                                    case 500:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 500 - Internal server error" +
                                            "\r\nAn error occured on the server.";
                                        break;
                                    #endregion

                                    #region 501 Not implemented
                                    case 501:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 501 - Not implemented" +
                                            "\r\nThe server does not support the requested function.";
                                        break;
                                    #endregion

                                    #region 502 Bad gateway
                                    case 502:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 502 - Bad gateway" +
                                            "\r\nThe proxy server recieved a bad response from another proxy or the origin server.";
                                        break;
                                    #endregion

                                    #region 503  Service unavailable
                                    case 503:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 503 - Service unavailable" +
                                            "\r\nThe server is temporarily unavailable, likely due to high load or maintenance.";
                                        break;
                                    #endregion

                                    #region 504 Gateway timeout
                                    case 504:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 504 - Gateway timeout" +
                                            "\r\nAn intermediate proxy server timed out while waiting for a response from another proxy or the origin server.";
                                        break;
                                    #endregion

                                    #region 505 Http version not supported
                                    case 505:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 505 - Http version not supported" +
                                            "\r\nThe requested HTTP version is not supported by the server.";
                                        break;
                                    #endregion
                                    #endregion
                                }
                            }
                            break;
                        #endregion
                        #region ConnectionClosed
                        case WebExceptionStatus.ConnectionClosed:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nConnection closed" +
                                            "\r\nThe connection was prematurely closed.";
                            break;
                        #endregion
                        #region TrustFailure
                        case WebExceptionStatus.TrustFailure:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nTrust failure" +
                                            "\r\nA server certificate could not be validated.";
                            break;
                        #endregion
                        #region SecureChannelFailure
                        case WebExceptionStatus.SecureChannelFailure:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nSecure channel failure" +
                                            "\r\nAn error occurred while establishing a connection using SSL.";
                            break;
                        #endregion
                        #region ServerProtocolViolation
                        case WebExceptionStatus.ServerProtocolViolation:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nServer protocol violation" +
                                            "\r\nThe server response was not a valid HTTP response.";
                            break;
                        #endregion
                        #region KeepAliveFailure
                        case WebExceptionStatus.KeepAliveFailure:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nKeep alive failure" +
                                            "\r\nThe connection for a request that specifies the Keep-alive header was closed unexpectedly.";
                            break;
                        #endregion
                        #region Pending
                        case WebExceptionStatus.Pending:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nPending" +
                                            "\r\nAn internal asynchronous request is pending.";
                            break;
                        #endregion
                        #region Timeout
                        case WebExceptionStatus.Timeout:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nTimeout" +
                                            "\r\nNo response was received during the time-out period for a request.";
                            break;
                        #endregion
                        #region ProxyNameResolutionFailure
                        case WebExceptionStatus.ProxyNameResolutionFailure:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nProxy name resolution failure" +
                                            "\r\nThe name resolver service could not resolve the proxy host name.";
                            break;
                        #endregion
                        #region UnknownError
                        case WebExceptionStatus.UnknownError:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nUnknown error" +
                                            "\r\nAn exception of unknown type has occurred.";
                            break;
                        #endregion
                        #region MessageLengthLimitExceeded
                        case WebExceptionStatus.MessageLengthLimitExceeded:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nMessage length limit exceeded" +
                                            "\r\nA message was received that exceeded the specified limit when sending a request or receiving a response from the server.";
                            break;
                        #endregion
                        #region CacheEntryNotFound
                        case WebExceptionStatus.CacheEntryNotFound:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nCache entry not found" +
                                            "\r\nThe specified cache entry was not found.";
                            break;
                        #endregion
                        #region RequestProhibitedByCachePolicy
                        case WebExceptionStatus.RequestProhibitedByCachePolicy:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nRequest prohibited by cache policy" +
                                            "\r\nThe request was not permitted by the cache policy.";
                            break;
                        #endregion
                        #region RequestProhibitedByProxy
                        case WebExceptionStatus.RequestProhibitedByProxy:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nRequest prohibited by proxy" +
                                            "\r\nThis request was not permitted by the proxy.";
                            break;
                        #endregion

                    }
                }

                if (UseCustomDescription) {
                    CustomDescriptionBuffer += WebException.InnerException + "\r\n\r\nStackTrace:\r\n" +
                                               WebException.StackTrace;
                }

                ExceptionDisplay.SetCustomDescription = UseCustomDescription;
                ExceptionDisplay.CustomDescription = CustomDescriptionBuffer;
                ExceptionDisplay.ShowDialog();
            }
        }
        /// <summary>
        /// Reports any web errors that are caught, and enables the Retry option.
        /// </summary>
        /// <param name="WebException">The WebException that was caught</param>
        /// <param name="WebsiteAddress">The URL that (might-have) caused the problem</param>
        /// <param name="ErrorClass">The class that the WebException occured in</param>
        /// <returns></returns>
        public static bool ReportWebExceptionWithRetry(WebException WebException, string WebsiteAddress, string ErrorClass) {
            string OutputFile = string.Empty;
            string CustomDescriptionBuffer = string.Empty;
            bool UseCustomDescription = false;

            using (frmException ExceptionDisplay = new frmException()) {
                ExceptionDisplay.ReportedWebException = WebException;
                ExceptionDisplay.FromLanguage = false;

                if ((int)WebException.Status == 418) {
                    UseCustomDescription = true;
                    CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                    "\r\n\r\nCannot brew coffee" +
                                    "\r\nI'm a teapot";
                }
                else {
                    switch (WebException.Status) {
                        #region NameResolutionFailure
                        case WebExceptionStatus.NameResolutionFailure:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nName resolution failure" +
                                            "\r\nThe name resolver service could not resolve the host name.";
                            break;
                        #endregion
                        #region ConnectFailure
                        case WebExceptionStatus.ConnectFailure:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nConnection failure" +
                                            "\r\nThe remote service point could not be contacted at the transport level.";
                            break;
                        #endregion
                        #region RecieveFailure
                        case WebExceptionStatus.ReceiveFailure:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nRecieve failure" +
                                            "\r\nA complete response was not received from the remote server.";
                            break;
                        #endregion
                        #region SendFailure
                        case WebExceptionStatus.SendFailure:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nSend failure" +
                                            "\r\nA complete response could not be sent to the remote server.";
                            break;
                        #endregion
                        #region PipelineFailure
                        case WebExceptionStatus.PipelineFailure:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nPipeline failure" +
                                            "\r\nThe request was a piplined request and the connection was closed before the response was received.";
                            break;
                        #endregion
                        #region RequestCanceled
                        case WebExceptionStatus.RequestCanceled:
                            return false;
                        #endregion
                        #region ProtocolError
                        case WebExceptionStatus.ProtocolError:
                            var WebResponse = WebException.Response as HttpWebResponse;

                            if (WebResponse != null) {
                                UseCustomDescription = true;
                                switch ((int)WebResponse.StatusCode) {
                                    #region StatusCodes
                                    #region default / unspecified
                                    default:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned " + WebResponse.StatusCode.ToString() +
                                            "\r\n" + WebResponse.StatusDescription.ToString();
                                        break;
                                    #endregion

                                    #region 301 Moved / Moved permanently
                                    case 301:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 301 - Moved / Moved permanently" +
                                            "\r\nThe requested information has been moved to the URI specified in the Location header.";
                                        break;
                                    #endregion

                                    #region 400 Bad request
                                    case 400:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 400 - Bad request" +
                                            "\r\nThe request could not be understood by the server.";
                                        break;
                                    #endregion

                                    #region 401 Unauthorized
                                    case 401:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 401 - Unauthorized" +
                                            "\r\nThe requested resource requires authentication.";
                                        break;
                                    #endregion

                                    #region 402 Payment required
                                    case 402:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 402 - Payment required" +
                                            "\r\nPayment is required to view this content.\r\nThis status code isn't natively used.";
                                        break;
                                    #endregion

                                    #region 403 Forbidden
                                    case 403:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 403 - Forbidden" +
                                            "\r\nYou do not have permission to view this file.";
                                        break;
                                    #endregion

                                    #region 404 Not found
                                    case 404:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 404 - Not found" +
                                            "\r\nThe file does not exist on the server.";
                                        break;
                                    #endregion

                                    #region 405 Method not allowed
                                    case 405:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 405 - Method not allowed" +
                                            "\r\nThe request method (GET) is not allowed on the requested resource.";
                                        break;
                                    #endregion

                                    #region 406 Not acceptable
                                    case 406:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 406 - Not acceptable" +
                                            "\r\nThe client has indicated with Accept headers that it will not accept any of the available representations from the resource.";
                                        break;
                                    #endregion

                                    #region 407 Proxy authentication required
                                    case 407:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 407 - Proxy authentication required" +
                                            "\r\nThe requested proxy requires authentication.";
                                        break;
                                    #endregion

                                    #region 408 Request timeout
                                    case 408:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 408 - Request timeout" +
                                            "\r\nThe client did not send a request within the time the server was expection the request.";
                                        break;
                                    #endregion

                                    #region 409 Conflict
                                    case 409:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 409 - Conflict" +
                                            "\r\nThe request could not be carried out because of a conflict on the server.";
                                        break;
                                    #endregion

                                    #region 410 Gone
                                    case 410:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 410 - Gone" +
                                            "\r\nThe requested resource is no longer available.";
                                        break;
                                    #endregion

                                    #region 411 Length required
                                    case 411:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 410 - Length required" +
                                            "\r\nThe required Content-length header is missing.";
                                        break;
                                    #endregion

                                    #region 412 Precondition failed
                                    case 412:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 412 - Precondition failed" +
                                            "\r\nA condition set for this request failed, and the request cannot be carried out.";
                                        break;
                                    #endregion

                                    #region 413 Request entity too large
                                    case 413:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 413 - Request entity too large" +
                                            "\r\nThe request is too large for the server to process.";
                                        break;
                                    #endregion

                                    #region 414 Request uri too long
                                    case 414:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 414 - Request uri too long" +
                                            "\r\nThe uri is too long.";
                                        break;
                                    #endregion

                                    #region 415 Unsupported media type
                                    case 415:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 415 - Unsupported media type" +
                                            "\r\nThe request is an unsupported type.";
                                        break;
                                    #endregion

                                    #region 416 Requested range not satisfiable
                                    case 416:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 416 - Requested range not satisfiable" +
                                            "\r\nThe range of data requested from the resource cannot be returned.";
                                        break;
                                    #endregion

                                    #region 417 Expectation failed
                                    case 417:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 417 - Expectation failed" +
                                            "\r\nAn expectation given in an Expect header could not be met by the server.";
                                        break;
                                    #endregion

                                    #region 426 Upgrade required
                                    case 426:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 426 - Upgrade required" +
                                            "\r\nNo information is available about this error code.";
                                        break;
                                    #endregion

                                    #region 500 Internal server error
                                    case 500:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 500 - Internal server error" +
                                            "\r\nAn error occured on the server.";
                                        break;
                                    #endregion

                                    #region 501 Not implemented
                                    case 501:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 501 - Not implemented" +
                                            "\r\nThe server does not support the requested function.";
                                        break;
                                    #endregion

                                    #region 502 Bad gateway
                                    case 502:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 502 - Bad gateway" +
                                            "\r\nThe proxy server recieved a bad response from another proxy or the origin server.";
                                        break;
                                    #endregion

                                    #region 503  Service unavailable
                                    case 503:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 503 - Service unavailable" +
                                            "\r\nThe server is temporarily unavailable, likely due to high load or maintenance.";
                                        break;
                                    #endregion

                                    #region 504 Gateway timeout
                                    case 504:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 504 - Gateway timeout" +
                                            "\r\nAn intermediate proxy server timed out while waiting for a response from another proxy or the origin server.";
                                        break;
                                    #endregion

                                    #region 505 Http version not supported
                                    case 505:
                                        CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nThe address returned 505 - Http version not supported" +
                                            "\r\nThe requested HTTP version is not supported by the server.";
                                        break;
                                    #endregion
                                    #endregion
                                }
                            }
                            break;
                        #endregion
                        #region ConnectionClosed
                        case WebExceptionStatus.ConnectionClosed:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nConnection closed" +
                                            "\r\nThe connection was prematurely closed.";
                            break;
                        #endregion
                        #region TrustFailure
                        case WebExceptionStatus.TrustFailure:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nTrust failure" +
                                            "\r\nA server certificate could not be validated.";
                            break;
                        #endregion
                        #region SecureChannelFailure
                        case WebExceptionStatus.SecureChannelFailure:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nSecure channel failure" +
                                            "\r\nAn error occurred while establishing a connection using SSL.";
                            break;
                        #endregion
                        #region ServerProtocolViolation
                        case WebExceptionStatus.ServerProtocolViolation:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nServer protocol violation" +
                                            "\r\nThe server response was not a valid HTTP response.";
                            break;
                        #endregion
                        #region KeepAliveFailure
                        case WebExceptionStatus.KeepAliveFailure:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nKeep alive failure" +
                                            "\r\nThe connection for a request that specifies the Keep-alive header was closed unexpectedly.";
                            break;
                        #endregion
                        #region Pending
                        case WebExceptionStatus.Pending:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nPending" +
                                            "\r\nAn internal asynchronous request is pending.";
                            break;
                        #endregion
                        #region Timeout
                        case WebExceptionStatus.Timeout:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nTimeout" +
                                            "\r\nNo response was received during the time-out period for a request.";
                            break;
                        #endregion
                        #region ProxyNameResolutionFailure
                        case WebExceptionStatus.ProxyNameResolutionFailure:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nProxy name resolution failure" +
                                            "\r\nThe name resolver service could not resolve the proxy host name.";
                            break;
                        #endregion
                        #region UnknownError
                        case WebExceptionStatus.UnknownError:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nUnknown error" +
                                            "\r\nAn exception of unknown type has occurred.";
                            break;
                        #endregion
                        #region MessageLengthLimitExceeded
                        case WebExceptionStatus.MessageLengthLimitExceeded:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nMessage length limit exceeded" +
                                            "\r\nA message was received that exceeded the specified limit when sending a request or receiving a response from the server.";
                            break;
                        #endregion
                        #region CacheEntryNotFound
                        case WebExceptionStatus.CacheEntryNotFound:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nCache entry not found" +
                                            "\r\nThe specified cache entry was not found.";
                            break;
                        #endregion
                        #region RequestProhibitedByCachePolicy
                        case WebExceptionStatus.RequestProhibitedByCachePolicy:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nRequest prohibited by cache policy" +
                                            "\r\nThe request was not permitted by the cache policy.";
                            break;
                        #endregion
                        #region RequestProhibitedByProxy
                        case WebExceptionStatus.RequestProhibitedByProxy:
                            UseCustomDescription = true;
                            CustomDescriptionBuffer += "A WebException in " + ErrorClass + " occured at " + WebsiteAddress +
                                            "\r\n\r\nRequest prohibited by proxy" +
                                            "\r\nThis request was not permitted by the proxy.";
                            break;
                        #endregion

                    }
                }

                if (UseCustomDescription) {
                    CustomDescriptionBuffer += WebException.InnerException + "\r\n\r\nStackTrace:\r\n" +
                                               WebException.StackTrace;
                }

                ExceptionDisplay.SetCustomDescription = UseCustomDescription;
                ExceptionDisplay.CustomDescription = CustomDescriptionBuffer;
                ExceptionDisplay.AllowRetry = true;

                switch (ExceptionDisplay.ShowDialog()) {
                    case System.Windows.Forms.DialogResult.Retry:
                        return true;

                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Reports any general exceptions that are caught.
        /// </summary>
        /// <param name="Exception">The Exception that was caught</param>
        /// <param name="ErrorClass">The class that the exception occured in</param>
        /// <param name="IsWriteToFile">Determine if the error gets logged into error.log</param>
        public static void ReportException(Exception Exception, string ErrorClass, bool IsWriteToFile = true) {
            string OutputFile = string.Empty;

            using (frmException ExceptionDisplay = new frmException()) {
                ExceptionDisplay.ReportedException = Exception;
                ExceptionDisplay.FromLanguage = false;
                ExceptionDisplay.ShowDialog();
            }
        }
        /// <summary>
        /// Reports any general exceptions that are caught, and enables the Retry option.
        /// </summary>
        /// <param name="Exception">The Exception that was caught</param>
        /// <param name="ErrorClass">The class that the exception occured in</param>
        /// <param name="IsWriteToFile">Determine if the error gets logged into error.log</param>
        public static bool ReportExceptionWithRetry(Exception Exception, string ErrorClass, bool IsWriteToFile = true) {
            string OutputFile = string.Empty;

            using (frmException ExceptionDisplay = new frmException()) {
                ExceptionDisplay.ReportedException = Exception;
                ExceptionDisplay.FromLanguage = false;
                ExceptionDisplay.AllowRetry = true;
                switch (ExceptionDisplay.ShowDialog()) {
                    case System.Windows.Forms.DialogResult.Retry:
                        return true;
                        
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Reports a custom-styled exception that is designed in the method that calls it.
        /// </summary>
        /// <param name="Exception">The (stylized) exception</param>
        /// <param name="ErrorClass">The class that it originated from</param>
        public static void ReportCustomException(string Exception, string ErrorClass) {
            using (frmException ExceptionDisplay = new frmException()) {
                ExceptionDisplay.SetCustomDescription = true;
                ExceptionDisplay.CustomDescription = Exception + "\r\n\r\nThis exception occured at: " + ErrorClass;
                ExceptionDisplay.ShowDialog();
            }
        }
        /// <summary>
        /// Reports a custom-styled exception that is designed in the method that calls it, and enables the Retry option.
        /// </summary>
        /// <param name="Exception">The (stylized) exception</param>
        /// <param name="ErrorClass">The class that it originated from</param>
        public static bool ReportCustomExceptionWithRetry(string Exception, string ErrorClass) {
            using (frmException ExceptionDisplay = new frmException()) {
                ExceptionDisplay.SetCustomDescription = true;
                ExceptionDisplay.CustomDescription = Exception + "\r\n\r\nThis exception occured at: " + ErrorClass;
                ExceptionDisplay.AllowRetry = true;
                switch (ExceptionDisplay.ShowDialog()) {
                    case System.Windows.Forms.DialogResult.Retry:
                        return true;

                    default:
                        return false;
                }
            }
        }

        public static void WriteToFile(string Buffer) {
            try {
                string FileName = string.Format("\\error_{0}.log", DateTime.Now);
                System.IO.File.WriteAllText(FileName, Buffer);
            }
            catch (Exception ex) { ReportException(ex, "ErrorLog.cs", false); }
        }
    }
}