namespace CVhantering.ValidateFolder
{
    public class ResultHelper<T> 
    {
        public bool IsSuccess { get; set; } // Boolean to check if the operation was successful
        public T Data { get; set; } // Where T is the type of data, can be any type
        public IEnumerable<string> Errors { get; set; } // List of errors

        // Return a new instance of ResultHelper with IsSuccess set to true and Data set to the provided data
        public static ResultHelper<T> Success(T data) 
        {
            return new ResultHelper<T> { IsSuccess = true, Data = data };
        }
        // Return a new instance of ResultHelper with IsSuccess set to false and Errors set to the provided errors
        public static ResultHelper<T> Failure(IEnumerable<string> errors)
        {
            return new ResultHelper<T> { IsSuccess = false, Errors = errors };
        }
    }
}
