namespace GAME.PhotoRanking.Models
{
    public class Response<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static async Task<Response<T>> DoAsync(Func<Response<T>, Task> action)
        {
            Response<T> response = new Response<T>();
            try
            {
                await action(response);
            }
            catch (Exception e)
            {
                response.Code = -1;
                response.Message = e.Message;
            }

            return response;
        }

        public T GetResult()
        {
            if(Code == 0)
                return Data;
            throw new Exception(Message);
        }

        public void Throw(int code, string message) =>
            throw new Exception(message);

        public void Throw(string message) =>
            Throw(-1, message);

        public void ThrowIfNull(object? obj, string message)
        {
            if (obj == null)
                Throw(message);
        }

        public void ThrowIfEmptyArray<T>(IEnumerable<T> values, string message)
        {
            if (!values.Any())
                Throw(message);
        }
    }
}
