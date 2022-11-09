using ds.core.common;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.IO;

namespace ds.portal.ui.Services
{
    public class AssetsService : IAssetsService
    {
        /// <summary>
        /// The hosting environment
        /// </summary>
        private readonly IHostingEnvironment _hostingEnvironment;
        //If migrating to .NET CORE 3 see the following link
        //https://stackoverflow.com/questions/49398965/what-is-the-equivalent-of-server-mappath-in-asp-net-core        
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetsService"/> class.
        /// </summary>
        /// <param name="hostingEnvironment">The hosting environment.</param>
        public AssetsService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        /// <summary>
        /// Loads from json.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public T LoadFromJson<T>(string filePath)
        {
            using (var sr = new StreamReader(filePath))
            {
                return JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
            }
        }
    }
}
