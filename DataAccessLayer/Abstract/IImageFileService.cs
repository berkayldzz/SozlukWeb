using EntityLayer.Concrete;
using System.Collections.Generic;

namespace DataAccessLayer.Abstract
{
    public interface IImageFileService
    {
        List<ImageFile> GetList();

    }
}
