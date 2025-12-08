namespace Core.Exceptions
{
    public class ConnectionBusinessException : BaseException
    {
        public ConnectionBusinessException(EntityBaseException entityBaseException) : base(entityBaseException)
        {
        }
    }
}
