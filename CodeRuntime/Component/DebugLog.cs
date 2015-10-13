#if Component
using CodeRuntime;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
[assembly: PreApplicationStartMethod(typeof(DebugLog), "Register")]
namespace System
{
    public sealed class DebugLog
    {

#if DEBUG
               static readonly bool _defaultRegister = true;
#else
        static readonly bool _defaultRegister = false;
#endif

        static bool _isRegister = false;
        public static void Register()
        {
            if (_isRegister)
            {
                return;
            }
            //是否注册模块
            bool register = false;
            if (!bool.TryParse(ConfigurationManager.AppSettings["DebugLog"], out register))
            {
                register = _defaultRegister;
            }
            if (register)
            {
                HttpApplication.RegisterModule(typeof(DebugLogHttpModule));
            }
            _isRegister = true;
        }

        [ThreadStatic]
        static DebugLog _current;


        public static DebugLog Current
        {
            get
            {
                if (_current == null)
                {
                    if (!_isRegister)
                    {
                        throw new NotImplementedException("请在应用程序初始化前调用 DebugLog.Register 方法");
                    }
                    _current = new DebugLog();
                }
                return _current;
            }
        }

        public static string Message
        {
            get
            {
                return Current.InternalMessage;
            }
        }

        string InternalMessage
        {
            get
            {
                return _sb.ToString();
            }
        }

        public static void Write(string message)
        {
            Current.InternalWrite(message);
        }

        public static void WriteLine(string message)
        {
            Current.InternalWriteLine(message);
        }

        public static void Write(object value)
        {
            Write(value.ToString());
        }
        public static void WriteLine(object value)
        {
            WriteLine(value.ToString());
        }


        StringBuilder _sb = new StringBuilder();

        void InternalWrite(string message)
        {
            _sb.Append(message);
        }

        void InternalWriteLine(string message)
        {
            _sb.AppendLine(message);
        }

        internal static void Dispose()
        {
            _current = null;
        }
    }
}
#endif