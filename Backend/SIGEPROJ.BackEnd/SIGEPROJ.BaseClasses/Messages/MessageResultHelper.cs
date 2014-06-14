using System;
using System.Collections.Generic;

namespace SIGEPROJ.BaseClasses.Messages
{
    /// <summary>
    /// Helper de MessageResult
    /// </summary>
    public static class MessageResultHelper
    {
        #region Errors

        /// <summary>
        /// Indica si tiene errores
        /// </summary>
        /// <param name="listOfMessageResult"></param>
        /// <returns></returns>
        public static bool HasErrors(this IEnumerable<MessageResult> listOfMessageResult)
        {
           foreach( var message in listOfMessageResult)
           {
               if(message.Kind == MessageKind.Error)
               {
                   return true;
               }
           }
           return false;
        }

        /// <summary>
        /// Cuenta errores
        /// </summary>
        /// <param name="listOfMessageResult"></param>
        /// <returns></returns>
        public static int CountErrors(this IEnumerable<MessageResult> listOfMessageResult)
        {
           int cantErrors = 0;
           foreach( var message in listOfMessageResult)
           {
               if(message.Kind == MessageKind.Error)
               {
                   cantErrors++;
               }
           }
           return cantErrors;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string ShowErrorString(this IEnumerable<MessageResult> listOfMessageResult)
        {
            string strError = string.Empty;
            foreach (var message in listOfMessageResult)
            {
                if (message.Kind == MessageKind.Error)
                {
                    strError += message.MessageNum + " - " + message + Environment.NewLine;
                }
            }
            return strError;
        }
        #endregion

        #region Warnings
        /// <summary>
        /// Indica si contiene Warnings
        /// </summary>
        /// <param name="listOfMessageResult"></param>
        /// <returns></returns>
        public static bool HasWarnings(this IEnumerable<MessageResult> listOfMessageResult)
        {
            foreach (var message in listOfMessageResult)
            {
                if (message.Kind == MessageKind.Warning)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Cuenta los Warnings
        /// </summary>
        /// <param name="listOfMessageResult"></param>
        /// <returns></returns>
        public static int CountWarnings(this IEnumerable<MessageResult> listOfMessageResult)
        {
            int cantErrors = 0;
            foreach (var message in listOfMessageResult)
            {
                if (message.Kind == MessageKind.Warning)
                {
                    cantErrors++;
                }
            }
            return cantErrors;
        }
        #endregion
    }
}