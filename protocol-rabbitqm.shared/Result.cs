using System.Text.Json.Serialization;

namespace protocol.rabbitqm.shared
{
    public class Result<T>
    {
        public T data { get; set; }
        private string _xMessages;
        public string xmessage { get { return GetMessageList(); } set { _xMessages = value; } }
        [JsonIgnore]
        public List<string> messages { get; set; } = new List<string>();
        public bool success { get; set; }

        private string GetMessageList()
        {
            if (messages.Count > 0)
            {
                var _item = "";
                foreach (var item in messages)
                    _item += _item + item + (_item.Length == 0 ? "" : Environment.NewLine);
                return _item;
            }
            else
                return "";
        }

        public static Result<T> Fail()
        {
            return new Result<T> { success = false };
        }

        public static Result<T> Fail(T data)
        {
            return new Result<T> { success = false, data = data };
        }

        public static Result<T> Fail(Exception e)
        {
            if (e.Message != null)
                return new Result<T> { success = false, messages = new List<string> { e.Message } };
            else
                return new Result<T> { success = false, messages = new List<string> { e.InnerException.Message } };
        }


        private static string AdminMessage(string sender, bool admin)
        {
            if (admin)
                return $"{sender} Contate o administrador do sistema.";
            else
                return sender;
        }

        public static Result<T> Fail(string message, bool admin = false)
        {
            message = AdminMessage(message, admin);
            return new Result<T> { success = false, messages = new List<string> { message } };
        }

        public static Result<T> Fail(List<string> messages)
        {
            return new Result<T> { success = false, messages = messages };
        }

        public static Task<Result<T>> FailAsync()
        {
            return Task.FromResult(Fail());
        }

        public static Task<Result<T>> FailAsync(string message)
        {
            return Task.FromResult(Fail(message));
        }

        public static Task<Result<T>> FailAsync(List<string> messages)
        {
            return Task.FromResult(Fail(messages));
        }

        public static Result<T> Success()
        {
            return new Result<T> { success = true };
        }

        public static Result<T> Success(T data)
        {
            return new Result<T> { success = true, data = data };
        }

        public static Result<T> Success(T data, string message = "ok")
        {
            return new Result<T> { success = true, data = data, xmessage = message };
        }

        public static Result<T> Success(string message)
        {
            return new Result<T> { success = true, messages = new List<string> { message } };
        }

        public static Result<T> SuccessData(T data)
        {
            return new Result<T> { success = true, xmessage = "ok", data = data };
        }


        public static Task<Result<T>> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public static Task<Result<T>> SuccessAsync(T data, string message = "ok")
        {
            return Task.FromResult(Success(data, message));
        }

        public static Task<Result<T>> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));
        }
    }
}
