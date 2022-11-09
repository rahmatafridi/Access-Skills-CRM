namespace ds.core.common
{
    public interface IAssetsService
    {
        /// <summary>
        /// Loads from json.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        T LoadFromJson<T>(string filePath);
    }
}
