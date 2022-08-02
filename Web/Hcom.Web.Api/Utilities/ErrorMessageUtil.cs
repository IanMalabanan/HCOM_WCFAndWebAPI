using Hcom.Web.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hcom.Web.Api.Utilities
{
    public class ErrorMessageUtil
    {
        public static string GetFullExceptionMessage(Exception ex)
        {
            string msg;

            if (ex.InnerException != null)
                msg = ex.InnerException.Message;
            else
                msg = ex.Message;
            return msg;
        }

        public static string GetFullExceptionMessage(ApplicationException ex)
        {
            string msg;

            if (ex.InnerException != null)
                msg = ex.InnerException.Message;
            else
                msg = ex.Message;
            return msg;
        }

        public static string GetFullExceptionMessage(NullReferenceException ex)
        {
            string msg;

            if (ex.InnerException != null)
                msg = ex.InnerException.Message;
            else
                msg = ex.Message;
            return msg;
        }

        public static string GetFullExceptionMessage(SystemException ex)
        {
            string msg;

            if (ex.InnerException != null)
                msg = ex.InnerException.Message;
            else
                msg = ex.Message;
            return msg;
        }

        public static string GetFullExceptionMessage(IndexOutOfRangeException ex)
        {
            string msg;

            if (ex.InnerException != null)
                msg = ex.InnerException.Message;
            else
                msg = ex.Message;
            return msg;
        }

        public static string GetFullExceptionMessage(StackOverflowException ex)
        {
            string msg;

            if (ex.InnerException != null)
                msg = ex.InnerException.Message;
            else
                msg = ex.Message;
            return msg;
        }

    }
}
