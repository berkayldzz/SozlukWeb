using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System.Collections.Generic;

namespace BusinessLayer.Concrete
{
    public class ImageFileManager : IImageFileService
    {
        IImageFileDal _imageFile;

        public ImageFileManager(IImageFileDal imageFile)
        {
            _imageFile = imageFile;
        }

        public List<ImageFile> GetList()
        {
            return _imageFile.List();
        }
    }
}
