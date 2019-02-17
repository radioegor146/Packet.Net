using System;
#if DEBUG
#endif

namespace PacketDotNet
{
    // For Release builds we disable logging by using this class
    // in place of a log4net logger
    internal class ILogInactive
    {
#if DEBUG
        public bool IsDebugEnabled
        {
            get { return false; }
        }

        public bool IsInfoEnabled
        {
            get { return false; }
        }

        public bool IsWarnEnabled
        {
            get { return false; }
        }

        public bool IsErrorEnabled
        {
            get { return false; }
        }

        public bool IsFatalEnabled
        {
            get { return false; }
        }
#endif

		[System.Diagnostics.ConditionalAttribute("NEVER")]
        public void Debug (object message)
        {
            throw new System.NotImplementedException();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void Debug (object message, System.Exception exception)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void DebugFormat(string format, params object[] args)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void DebugFormat (System.IFormatProvider provider, string format, params object[] args)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void DebugFormat (string format, object arg0)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void DebugFormat (string format, object arg0, object arg1)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void DebugFormat (string format, object arg0, object arg1, object arg2)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void Error (object message)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void Error (object message, System.Exception exception)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void ErrorFormat(string format, params object[] args)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void ErrorFormat (System.IFormatProvider provider, string format, params object[] args)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void ErrorFormat (string format, object arg0)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void ErrorFormat (string format, object arg0, object arg1)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void ErrorFormat (string format, object arg0, object arg1, object arg2)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void Fatal (object message)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void Fatal (object message, System.Exception exception)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void FatalFormat(string format, params object[] args)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void FatalFormat (System.IFormatProvider provider, string format, params object[] args)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void FatalFormat (string format, object arg0)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void FatalFormat (string format, object arg0, object arg1)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void FatalFormat (string format, object arg0, object arg1, object arg2)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void Info (object message)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void Info (object message, System.Exception exception)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void InfoFormat(string format, params object[] args)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void InfoFormat (System.IFormatProvider provider, string format, params object[] args)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void InfoFormat (string format, object arg0)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void InfoFormat (string format, object arg0, object arg1)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void InfoFormat (string format, object arg0, object arg1, object arg2)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void Warn (object message)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void Warn (object message, System.Exception exception)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void WarnFormat(string format, params object[] args)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void WarnFormat (System.IFormatProvider provider, string format, params object[] args)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void WarnFormat (string format, object arg0)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void WarnFormat (string format, object arg0, object arg1)
        {
            throw new System.NotImplementedException ();
        }

        [System.Diagnostics.ConditionalAttribute("NEVER")]
        public void WarnFormat (string format, object arg0, object arg1, object arg2)
        {
            throw new System.NotImplementedException ();
        }

        public static ILogInactive GetLogger(Type type)
        {
            return new ILogInactive();
        }
    }
}
