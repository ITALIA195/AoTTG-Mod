using System;

namespace Mod
{
    public class Log
    {
        private readonly string _message;
        private readonly ErrorType _type;
        private readonly DateTime _date;

        public Log(string message, ErrorType type, DateTime date)
        {
            _message = message;
            _type = type;
            _date = date;
        }

        public string Message => _message;
        public ErrorType Type => _type;
        public string Date => _date.ToString("yyyy-MM-dd HH:mm:ss");
        public override string ToString() => $"{_date} [{_type}] {_message}";
    }
}
