namespace ProjectTisa.Libs
{
    /// <summary>
    /// Class for create and send to ILogger log message about different event.
    /// </summary>
    public static class LogMessageCreator
    {
        /// <summary>
        /// Generate log message about create of new entity.
        /// </summary>
        /// <param name="logger">ILogger instance.</param>
        /// <param name="createdEntity">Entity created at some action. Gets type of it and write in log message.</param>
        public static void CreatedMessage(ILogger logger, object createdEntity)
        {
            logger.LogInformation("Created new element type of {type}", createdEntity.GetType());
        }
        /// <summary>
        /// Generate log message about delete of entity.
        /// </summary>
        /// <param name="logger">ILogger instance.</param>
        /// <param name="deleteEntity">Deleted entity.</param>
        public static void DeletedMessage(ILogger logger, object deleteEntity)
        {
            logger.LogInformation("Deleted element: \"{entity}\"", deleteEntity);
        }
        /// <summary>
        /// Generate log message about granting a token.
        /// </summary>
        /// <param name="logger">ILogger instance.</param>
        /// <param name="user">User entity of granted token.</param>
        public static void TokenGranted(ILogger logger, string username)
        {
            logger.LogInformation("Granted token to user - {username}", username);
        }
    }
}
