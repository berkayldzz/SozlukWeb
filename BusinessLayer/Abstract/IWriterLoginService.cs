using EntityLayer.Concrete;

namespace BusinessLayer.Abstract
{
    public interface IWriterLoginService
    {
        Writer GetWriter(string username, string password);
    }
}
