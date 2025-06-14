﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class Writer
    {
        [Key]
        public int WriterID { get; set; }

        [StringLength(50)]
        public string WriterName { get; set; }

        [StringLength(50)]
        public string WriterSurName { get; set; }

        [StringLength(250)]
        public string WriterImage { get; set; }

        [StringLength(100)]
        public string WriterAbout { get; set; }

        [StringLength(200)]
        public string WriterMail { get; set; }

        [StringLength(200)]
        public string WriterPassword { get; set; }

        [StringLength(50)]
        public string WriterTitle { get; set; }
        public bool WriterStatus { get; set; }

        // Yazar - Başlık İlişkisi (1 yazar birden fazla başlığa sahip olabilir)
        public ICollection<Heading> Headings { get; set; }

        // Yazar - İçerik İlişkisi (1 yazar birden fazla içeriğe sahip olabilir)
        public ICollection<Content> Contents { get; set; }

    }
}
