﻿using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class ImageFile
    {
        [Key]
        public int ImageID { get; set; }

        [StringLength(100)]
        public string ImageName { get; set; }

        [StringLength(250)]
        public string ImagePath { get; set; }
    }
}
