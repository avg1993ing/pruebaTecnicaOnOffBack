namespace Core.Exceptions
{
    public class BadRequestBusinessException : BaseException
    {
        public BadRequestBusinessException(EntityBaseException entityBaseException) : base(entityBaseException)
        {
        }
        public BadRequestBusinessException(EntityBaseException exception, string information) : base($"{exception.Message} : {information}")
        {
        }
    }
}
