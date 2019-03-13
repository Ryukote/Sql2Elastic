namespace ElasticSQLServer.Contracts.Mapping
{
    /// <summary>
    /// Mapping data types from source to destination.
    /// </summary>
    public interface IMapper
    {
        /// <summary>
        /// Mapping data types from source to destination.
        /// </summary>
        /// <param name="key">Source data type.</param>
        /// <returns>Returns destination data type.</returns>
        string GetMappedType(string key);
    }
}
