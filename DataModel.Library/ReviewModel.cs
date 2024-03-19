using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Library
{
   public class ReviewModel
    {
        public int ReviewId { get; set; }

        [Display(Name ="Organization name")]
        public string CompanyName { get; set; }
        [DataType(DataType.MultilineText)]

        [Display(Name ="Interview feedback")]
        public string Content { get; set; }

        [Display(Name ="Interview rating")]
        public int Rating { get; set; }
        public DateTime TimeStamb { get; set; }
        public UserModel Author  { get; set; }
        public int UserId { get; set; }
    }
}

