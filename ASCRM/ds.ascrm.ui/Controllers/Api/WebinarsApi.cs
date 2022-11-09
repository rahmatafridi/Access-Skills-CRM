using LogMeIn.GoToWebinar.Api.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// Summary description for WebinarsApi
/// </summary>
public class WebinarsApi
{
    public class CreatedWebinar
    {
        /// <summary>
        /// The unique key of the webinar.
        /// </summary>
        public string webinarKey;
    }

    public class DateTimeRange
    {
        public DateTime? startTime { get; set; }

        /// <summary>
        /// The ending time of an interval, e.g. 2015-07-13T22:00:00Z
        /// </summary>
        public DateTime? endTime { get; set; }


        /// <summary>
        /// Returns a string representing the current object.
        /// </summary>
        /// <returns>The Json serialization of the current object.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class DateTimeRange {\n");
            var startTimeString = startTime.Stringify();
            if (!string.IsNullOrEmpty(startTimeString))
                sb.AppendFormat("  startTime: {0}\n", startTimeString);
            var endTimeString = endTime.Stringify();
            if (!string.IsNullOrEmpty(endTimeString))
                sb.AppendFormat("  endTime: {0}\n", endTimeString);
            sb.Append("}\n");
            return sb.ToString();
        }
    }
    public class WebinarReqCreate
    {
        public string subject { get; set; }

        /// <summary>
        /// A short description of the webinar (2048 characters maximum)
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// Array with startTime and endTime for webinar. Since this call creates single session webinars, the array can only contain a single pair of startTime and endTime
        /// </summary>
        public List<DateTimeRange> times { get; set; }

        /// <summary>
        /// The time zone where the webinar is taking place (must be a valid time zone ID, see https://goto-developer.logmein.com/time-zones). If this parameter is not passed, the timezone of the organizer's profile will be used
        /// </summary>
        public string timeZone { get; set; }

        /// <summary>
        /// Specifies the webinar type. The default type value is \"single_session\", which is used to create a single webinar session. The possible values are \"single_session\", \"series\", \"sequence\". If type is set to \"single_session\", a single webinar session is created. If type is set to \"series\", a webinar series is created. In this case 2 or more timeframes must be specified for each webinar. Example: \"times\": [{\"startTime\": \"...\", \"endTime\": \"...\"},{\"startTime\": \"...\", \"endTime\": \"...\"},{\"startTime\": \"...\", \"endTime\": \"...\"}. If type is set to \"sequence\" a sequence of webinars is created. The times object in the body must be replaced by the \"recurrenceStart\" object. Example: \"recurrenceStart\": {\"startTime\":\"2012-06-12T16:00:00Z\", \"endTime\": \"2012-06-12T17:00:00Z\" }.  The \"recurrenceEnd\" and \"recurrencePattern\" body parameter must be specified. Example: , \"recurrenceEnd\": \"2012-07-10\", \"recurrencePattern\": \"daily\".
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// A boolean flag indicating if the webinar is password protected or not.
        /// </summary>
        public bool? isPasswordProtected { get; set; }

        /// <summary>
        /// The recording asset with which the simulive webinar should be created from. In case the recordingasset was created as an online recording the simulive webinar settings, poll and surveys would be copied from the webinar whose session was recorded.
        /// </summary>
        public string recordingAssetKey { get; set; }

        /// <summary>
        /// A boolean flag indicating if the webinar should be ondemand
        /// </summary>
        public bool? isOndemand { get; set; }

        /// <summary>
        /// Specifies the experience type. The possible values are \"CLASSIC\", \"BROADCAST\" and \"SIMULIVE\".
        /// </summary>
        public string experienceType { get; set; }



        /// <summary>
        /// Returns a string representing the current object.
        /// </summary>
        /// <returns>The Json serialization of the current object.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class WebinarReqCreate {\n");
            var subjectString = subject.Stringify();
            if (!string.IsNullOrEmpty(subjectString))
                sb.AppendFormat("  subject: {0}\n", subjectString);
            var descriptionString = description.Stringify();
            if (!string.IsNullOrEmpty(descriptionString))
                sb.AppendFormat("  description: {0}\n", descriptionString);
            var timesString = times.Stringify();
            if (!string.IsNullOrEmpty(timesString))
                sb.AppendFormat("  times: {0}\n", timesString);
            var timeZoneString = timeZone.Stringify();
            if (!string.IsNullOrEmpty(timeZoneString))
                sb.AppendFormat("  timeZone: {0}\n", timeZoneString);
            var typeString = type.Stringify();
            if (!string.IsNullOrEmpty(typeString))
                sb.AppendFormat("  type: {0}\n", typeString);
            var isPasswordProtectedString = isPasswordProtected.Stringify();
            if (!string.IsNullOrEmpty(isPasswordProtectedString))
                sb.AppendFormat("  isPasswordProtected: {0}\n", isPasswordProtectedString);
            var recordingAssetKeyString = recordingAssetKey.Stringify();
            if (!string.IsNullOrEmpty(recordingAssetKeyString))
                sb.AppendFormat("  recordingAssetKey: {0}\n", recordingAssetKeyString);
            var isOndemandString = isOndemand.Stringify();
            if (!string.IsNullOrEmpty(isOndemandString))
                sb.AppendFormat("  isOndemand: {0}\n", isOndemandString);
            var experienceTypeString = experienceType.Stringify();
            if (!string.IsNullOrEmpty(experienceTypeString))
                sb.AppendFormat("  experienceType: {0}\n", experienceTypeString);
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    private string basePath = "https://api.getgo.com/G2W/rest/v2";
    public WebinarsApi()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    private string EscapeString(string str)
    {
        return str;
    }
    public static string Serialize(object obj)
    {
        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.NullValueHandling = NullValueHandling.Ignore;

        try
        {
            return obj != null ? JsonConvert.SerializeObject(obj, settings) : null;
        }
        catch (Exception e)
        {
            throw new Exception(e.ToString());
        }
    }
    private static string FixResponse(string json)
    {
        Regex rexDate = new Regex(@"(\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\+0{4})");

        json = rexDate.Replace(json, m =>
        {
            string match = m.ToString();
            return match.Substring(0, match.IndexOf(".")) + ".000Z";
        });

        Regex rexDateArray = new Regex(@"(\[\s*""\d{2}\/\d{2}\/\d{4} \d{2}:\d{2}:\d{2}""\s*,\s*""\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\+?0*Z?""\s*\])");

        json = rexDateArray.Replace(json, m =>
        {
            string match = m.ToString();
            int startPosition = match.IndexOf(",") + 1;

            return match.Substring(startPosition, match.IndexOf("]") - startPosition);
        });

        Regex rexMeetingId = new Regex(@"(""meetingId""\s*:\s*""[0-9]*-+[0-9-]*"")");

        json = rexMeetingId.Replace(json, m =>
        {
            string match = m.ToString();

            return match.Replace("-", "");
        });

        Regex rexDateCharsDayTwoDigits = new Regex(@"(.{3}\s.{3}\s\d{2}\s\d{2}:\d{2}:\d{2}\s\d{4})");

        json = rexDateCharsDayTwoDigits.Replace(json, m =>
        {
            string match = m.ToString();

            return DateTime.ParseExact(match, "ddd MMM d HH:mm:ss yyyy", CultureInfo.InvariantCulture).Stringify();
        });

        Regex rexDateCharsDayOneDigit = new Regex(@"(.{3}\s.{3}\s{2}\d{1}\s\d{2}:\d{2}:\d{2}\s\d{4})");

        json = rexDateCharsDayOneDigit.Replace(json, m =>
        {
            string match = m.ToString();

            return DateTime.ParseExact(match, "ddd MMM  d HH:mm:ss yyyy", CultureInfo.InvariantCulture).Stringify();
        });

        Regex rexDateCharsGlobal = new Regex(@"(.{3}\s.{3}\s{2}\d{2}\s\d{2}:\d{2}:\d{2}\s\d{4})");

        json = rexDateCharsGlobal.Replace(json, m =>
        {
            string match = m.ToString();

            return DateTime.ParseExact(match, "ddd MMM  dd HH:mm:ss yyyy", CultureInfo.InvariantCulture).Stringify();
        });

        return json;
    }

    /// <summary>
    ///
    /// </summary>
    public static object Deserialize(string json, Type type)
    {
        if (string.IsNullOrEmpty(json))
            return null;

        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.MissingMemberHandling = MissingMemberHandling.Ignore;

        try
        {
            return JsonConvert.DeserializeObject(json, type, settings);
        }
        catch (JsonSerializationException)
        {
            json = FixResponse(json);
            return JsonConvert.DeserializeObject(json, type, settings);
        }
        catch (JsonReaderException)
        {
            json = FixResponse(json);
            return JsonConvert.DeserializeObject(json, type, settings);
        }
        catch (IOException e)
        {
            throw new Exception(e.ToString());
        }
    }
    /// <summary>
    /// Create webinar
    /// Creates a single session webinar, a sequence of webinars or a series of webinars depending on the type field in the body: \"single_session\" creates a single webinar session, \"sequence\" creates a webinar with multiple meeting times where attendees are expected to be the same for all sessions, and \"series\" creates a webinar with multiple meetings times where attendees choose only one to attend. The default, if no type is declared, is single_session. A sequence webinar requires a \"recurrenceStart\" object consisting of a \"startTime\" and \"endTime\" key for the first webinar of the sequence, a \"recurrencePattern\" of \"daily\", \"weekly\", \"monthly\", and a \"recurrenceEnd\" which is the last date of the sequence (for example, 2016-12-01). A series webinar requires a \"times\" array with a discrete \"startTime\" and \"endTime\" for each webinar in the series. The call requires a webinar subject and description. The \"isPasswordProtected\" sets whether the webinar requires a password for attendees to join. If set to True, the organizer must go to Registration Settings at My Webinars (https://global.gotowebinar.com/webinars.tmpl) and add the password to the webinar, and send the password to the registrants. The response provides a numeric webinarKey in string format for the new webinar. Once a webinar has been created with this method, you can accept registrations. To create a scheduled simulive webinar set the \"experienceType\" as \"SIMULIVE\" along with the \"recordingAssetKey\". The \"recordingAssetKey\" is the unique identifier for the recording asset with which the simulive webinar should be created from. In case the asset was created as an online recording the simulive webinar settings, poll and surveys would be copied from the webinar whose session was recorded. The \"recordingAssetKey\" can be obtained using the new search recordingassets call. To create an on demand webinar set \"isOndemand\" to true along with the \"experienceType\" and the \"recordingAssetKey\". Simulive does not support sequence webinars.
    /// </summary>
    /// <param name="authorization">Access token</param>
    /// <param name="organizerKey">The key of the organizer</param>
    /// <param name="body">The webinar details</param>
    /// <returns>CreatedWebinar</returns>
    /// <exception cref="ApiException"></exception>
    public CreatedWebinar createWebinar(string authorization, long? organizerKey, WebinarReqCreate body)
    {
        if (authorization == null)
            throw new Exception("Required parameter authorization is null.");

        if (organizerKey == null)
            throw new Exception("Required parameter organizerKey is null.");

        if (body == null)
            throw new Exception("Required parameter body is null.");

        TestHttpRequest(body);

        // create path and map variables
        string path = "/organizers/{organizerKey}/webinars"
            .Replace("{format}", "json")
            .Replace("{" + "organizerKey" + "}", organizerKey.ToString());

        // query params
        Dictionary<String, String> queryParams = new Dictionary<String, String>();
        Dictionary<String, String> headerParams = new Dictionary<String, String>();
        Dictionary<String, object> formParams = new Dictionary<String, object>();

        headerParams.Add("Authorization", authorization);

        List<string> contentTypes = new List<string>();
        contentTypes.Add("application/json");
        string contentType = contentTypes.Count > 0 ? contentTypes[0] : "application/json";

        object response = InvokeAPI(basePath, path, "POST", queryParams, body, headerParams, formParams, typeof(CreatedWebinar), contentType);
        if (response != null)
        {
            return (CreatedWebinar)response;
        }
        return null;
    }
    public object InvokeAPI(string basePath, string path, string method, Dictionary<string, string> queryParams,
            object body, Dictionary<string, string> headerParams, Dictionary<string, object> formParams, Type responseType, string contentType)
    {
        if (responseType == typeof(byte[]))
            return InvokeAPI(basePath, path, method, true, queryParams, body, headerParams, formParams, contentType);

        string response = InvokeAPI(basePath, path, method, false, queryParams, body, headerParams, formParams, contentType) as string;
        return Deserialize(response, responseType);
    }

    private object InvokeAPI(string basePath, string path, string method, bool binaryResponse, Dictionary<string, string> queryParams, object body, Dictionary<string, string> headerParams, Dictionary<string, object> formParams, string contentType)
    {
        StringBuilder b = new StringBuilder();

        foreach (KeyValuePair<string, string> queryParamItem in queryParams)
        {
            string value = queryParamItem.Value;
            if (value == null)
                continue;
            b.Append(b.ToString().Length == 0 ? "?" : "&");
            b.Append(EscapeString(queryParamItem.Key)).Append("=").Append(EscapeString(value));
        }

        string querystring = b.ToString();

        basePath = basePath.EndsWith("/") ? basePath.Substring(0, basePath.Length - 1) : basePath;

        HttpWebRequest client = (HttpWebRequest)WebRequest.Create(basePath + path + querystring);
        client.Method = method;
        client.ContentType = contentType;

        byte[] formData = null;
        if (formParams.Count > 0)
        {
            if (client.ContentType == "application/x-www-form-urlencoded")
            {
                StringBuilder sb = new StringBuilder();
                foreach (KeyValuePair<string, object> kvp in formParams)
                {
                    if (kvp.Value != null && kvp.Value.ToString() != "")
                    {
                        if (sb.Length > 0)
                            sb.Append("&");

                        try
                        {
                            sb.Append(Uri.EscapeDataString(kvp.Key)).Append("=").Append(Uri.EscapeDataString(kvp.Value.ToString()));
                        }
                        catch (Exception)
                        {
                            // move on to next
                        }
                    }
                }

                body = sb.ToString();
                client.Accept = "application/json";
            }
            else
            {
                string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
                client.ContentType = "multipart/form-data; boundary=" + formDataBoundary;
                formData = GetMultipartFormData(formParams, formDataBoundary);
                client.ContentLength = formData.Length;
            }
        }
        else
        {
            client.Accept = "application/json";
        }

        foreach (KeyValuePair<string, string> headerParamsItem in headerParams)
        {
            if (headerParamsItem.Key == "Accept" && !string.IsNullOrEmpty(headerParamsItem.Value))
                client.Accept = headerParamsItem.Value;
            else
                client.Headers[headerParamsItem.Key] = headerParamsItem.Value;
        }
        client.Headers["CALLING_CLIENT"] = "GoToWebinar .NET SDK version 2.4.0";

        switch (method)
        {
            case "GET":
            case "HEAD":
                break;
            case "POST":
            case "PUT":
            case "DELETE":
                using (Stream requestStream = client.GetRequestStream())
                {
                    if (formData != null)
                        requestStream.Write(formData, 0, formData.Length);

                    StreamWriter swRequestWriter = new StreamWriter(requestStream);
                    swRequestWriter.Write(client.ContentType == "application/x-www-form-urlencoded" ? body : Serialize(body));
                    swRequestWriter.Close();
                }
                break;
            default:
                throw new Exception("Unexpected method " + method);
        }

        try
        {
            HttpWebResponse webResponse = (HttpWebResponse)client.GetResponse();
            if (webResponse.StatusCode != HttpStatusCode.OK
                && webResponse.StatusCode != HttpStatusCode.Created
                && webResponse.StatusCode != HttpStatusCode.NoContent
                && webResponse.StatusCode != HttpStatusCode.Accepted)
            {
                webResponse.Close();
                //throw new Exception((int)webResponse.StatusCode, webResponse.StatusDescription);
                throw new Exception(webResponse.StatusDescription);
            }

            if (binaryResponse)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    webResponse.GetResponseStream().CopyTo(memoryStream);                                       
                    return memoryStream.ToArray();
                }
            }
            else
            {
                using (StreamReader responseReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    string responseData = responseReader.ReadToEnd();
                    return responseData;
                }
            }
        }
        catch (WebException ex)
        {
            HttpWebResponse response = ex.Response as HttpWebResponse;
            int statusCode = 0;
            if (response != null)
            {
                statusCode = (int)response.StatusCode;
                response.Close();
            }
            //throw new ApiException(statusCode, ex.Message);
            throw new Exception(ex.Message);
        }
    }
    private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
    {
        Stream formDataStream = new System.IO.MemoryStream();
        bool needsCLRF = false;

        foreach (KeyValuePair<string, object> param in postParameters)
        {
            // Thanks to feedback from commenters, add a CRLF to allow multiple parameters to be added.
            // Skip it on the first parameter, add it to subsequent parameters.
            if (needsCLRF)
                formDataStream.Write(Encoding.UTF8.GetBytes("\r\n"), 0, Encoding.UTF8.GetByteCount("\r\n"));

            needsCLRF = true;

            if (param.Value is byte[])
            {
                string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n",
                    boundary,
                    param.Key,
                    "application/octet-stream");
                formDataStream.Write(Encoding.UTF8.GetBytes(postData), 0, Encoding.UTF8.GetByteCount(postData));

                // Write the file data directly to the Stream, rather than serializing it to a string.
                formDataStream.Write((param.Value as byte[]), 0, (param.Value as byte[]).Length);
            }
            else
            {
                string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                    boundary,
                    param.Key,
                    param.Value);
                formDataStream.Write(Encoding.UTF8.GetBytes(postData), 0, Encoding.UTF8.GetByteCount(postData));
            }
        }

        // Add the end of the request.  Start with a newline
        string footer = "\r\n--" + boundary + "--\r\n";
        formDataStream.Write(Encoding.UTF8.GetBytes(footer), 0, Encoding.UTF8.GetByteCount(footer));

        // Dump the Stream into a byte[]
        formDataStream.Position = 0;
        byte[] formData = new byte[formDataStream.Length];
        formDataStream.Read(formData, 0, formData.Length);
        formDataStream.Close();

        return formData;
    }

    public void TestHttpRequest(WebinarReqCreate body)
    {
        string url = "https://api.getgo.com/G2W/rest/v2/organizers/8813157848779510796/webinars";
        HttpWebRequest client = (HttpWebRequest)WebRequest.Create(url);

        string postData = Serialize(body);

        byte[] data = Encoding.UTF8.GetBytes(postData);

        string encoded = "Bearer EzWZUrSihPKgCTFTO6G4UcSBFUrh_QC3QkNj0yj7bDf3RpUja7kOoOXWyGnGYX_F3ITJ9Bcp1UfFk5N0bbIgvhWZ-1vFSfmDClUC.9f6baHxjbvLCEEJQj4jNMay8oUMp_7xjQUJi_Fg7eLOnid4OF1F3AItHw2G065_GRsi0iR3wAy8Jl5cE8Ai2MZYobcgchQ_icn1YLo0_WnLWFKBWf1KqwzZeyzUlaEi8oxa9IposXZ8GjEIvzi3Q0D4Ha90hfYAxbb_8SDN6YRoDF6PS2DcMfXJqevRCXGSu1KvgfRGF960LQFBsj2axUfZO7PWJs7qYy9kyIr0qzmWQENc1tbPDArS1r2tux9eFmCEY2acamBOEW7R3uFbjQmP4n2Um2hblNwVZildfZYPmG5kR_96tZNOcC9ZYE-u8cF49noRi_cdkpfAKj1XcgahofslYi3L2C68PX3wyR0fyVqlA6sIVjJGJkrxRpeBuOT00BxjToeBH12WgHkxcNT3thRyOmZAuvi7Y-4pfjGrFAblVnmogwLpF4dxVpqDGk2KNl5V7dB4kzmwuwi6iL002NqNErpXMR6tJCSQL7q_C6prUaP-1pM1kAr6UCDG6lg7wG7cfpY_hoY8IEn74NNG7626evcmNO6HfqiYTUWKdpwlHtP3-wkPJVzK1S_fYMuSYXlH_lskuqRXx8G9rsRAz1EGmsrMHvy3xbJtf4_Tk2YOXmSGSAdI6tB6CZP4v94hI9MKwg5RCoSQk2JMDy4pzDehnbarz728rl5iSyro5afcqPGJptzoEXymv34zmNhY.Z-JZxI-x3572otwf1-BbCDU3EDYM5S-KN3fYFuca_1Hd8oxkhiS1e8DCCFc35V50y4aSo9MmT5LZRJZjAaQmeTsipyoi5FP_XK4b";

        client.Headers.Add("Authorization", encoded);

        client.Method = "POST";
        client.Accept = "application/json";
        client.ContentType = "application/json";
        client.ContentLength = data.Length;

        using (Stream stream = client.GetRequestStream())
        {
            stream.Write(data, 0, data.Length);
        }
        try
        {
            HttpWebResponse response = (HttpWebResponse)client.GetResponse();

            //var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                //Response.Redirect("~/Portal/SalesAdvisor/ManageApplications.aspx");
            }
            else
            {
                
            }
        }
        catch (WebException ex)
        {
            throw new Exception(ex.ToString());
        }
    }

}