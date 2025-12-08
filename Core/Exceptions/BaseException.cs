namespace Core.Exceptions
{
    public class BaseException : Exception
    {
        public EntityBaseException exception { get; set; }
        public BaseException(EntityBaseException entityBaseException) : base(entityBaseException.Message)
        {
            this.exception = exception;
        }
        public BaseException(string mesaage) : base(mesaage)
        {

            mesaage = mesaage;
        }

        public BaseException(EntityBaseException exception, string information) : base($"{exception.Message} : {information}")
        {
            this.exception = exception;
        }
    }
}
