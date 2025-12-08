namespace Core.Exceptions
{
    public class InternalServerErrorBusinessExceprions : BaseException
    {
        public InternalServerErrorBusinessExceprions(EntityBaseException entityBaseException) : base(entityBaseException)
        {
        }
    }
}
