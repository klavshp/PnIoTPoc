namespace PnIotPoc.WebApi.Infrastructure.BusinessLogic
{
    public interface ICommandParameterTypeLogic
    {
        bool IsValid(string typeName, object value);
        object Get(string typeName, object value);
    }
}
