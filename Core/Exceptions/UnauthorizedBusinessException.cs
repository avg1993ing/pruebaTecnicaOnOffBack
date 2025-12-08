namespace Core.Exceptions
{
    public class UnauthorizedBusinessException : BaseException
    {
        public UnauthorizedBusinessException(EntityBaseException entityBaseException) : base(entityBaseException)
        {
        }
        public UnauthorizedBusinessException(string mesaage) : base(mesaage)
        {
        }
    }
}
