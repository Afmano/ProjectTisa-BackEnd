namespace ProjectTisa.Libs
{
    /// <summary>
    /// Class for create and send to ILogger log message about different event.
    /// </summary>
    public static class LogMessageCreator
    {
        /// <summary>
        /// Method to generate log message about create of new entity.
        /// </summary>
        /// <param name="logger">ILogger instance.</param>
        /// <param name="createdEntity">Entity created at some action. Gets type of it and write in log message.</param>
        public static void CreatedMessage(ILogger logger, object createdEntity)
        {
            logger.LogInformation("Created new element type of {type}", createdEntity.GetType());
        }
        /// <summary>
        /// Method to generate log message about delete of entity.
        /// </summary>
        /// <param name="logger">ILogger instance.</param>
        /// <param name="deleteId">Id of deleted entity.</param>
        public static void DeletedMessage(ILogger logger, uint deleteId)
        {
            logger.LogInformation("Deleted element at index {index}", deleteId);
        }
    }
}
