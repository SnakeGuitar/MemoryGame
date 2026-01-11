using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server.Shared
{
    public static class ServerExceptionManager
    {
        public static void SafeExecute(Action action, ILoggerManager logger, string operationName)
        {
            try
            {
                action();
            }
            catch (FaultException)
            {
                throw;
            }
            catch (EntityException ex)
            {
                logger.LogError($"[{operationName}] DB ERROR: {ex.Message} \n Stack: {ex.StackTrace}");
                throw new FaultException("Global_ServiceError_Database");
            }
            catch (SqlException ex)
            {
                logger.LogError($"[{operationName}] SQL ERROR: {ex.Message}");
                throw new FaultException("Global_ServiceError_Database");
            }
            catch (Exception ex)
            {
                logger.LogError($"[{operationName}] CRITICAL ERROR: {ex.Message} \n Stack: {ex.StackTrace}");
                throw new FaultException($"Internal server error in {operationName}. Please try again.");
            }
        }

        public static T SafeExecute<T>(Func<T> action, ILoggerManager logger, string operationName)
        {
            try
            {
                return action();
            }
            catch (FaultException) 
            {
                throw; 
            }
            catch (EntityException ex)
            {
                logger.LogError($"[{operationName}] DB ERROR: {ex.Message}");
                throw new FaultException("Global_ServiceError_Database");
            }
            catch (SqlException ex)
            {
                logger.LogError($"[{operationName}] SQL ERROR: {ex.Message}");
                throw new FaultException("Global_ServiceError_Database");
            }
            catch (Exception ex)
            {
                logger.LogError($"[{operationName}] CRITICAL ERROR: {ex.Message} \n Stack: {ex.StackTrace}");
                throw new FaultException($"Internal server error in {operationName}.");
            }
        }
    }
}