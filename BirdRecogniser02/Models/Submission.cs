﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BirdRecogniser02.Models
{
    public class Submission
    {
        [Key]
        public int SubmissionId { get; set; }

        [Required]
        [DisplayName("Bird Name")]
        [StringLength(50, MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Bird name should only contain alphabetic characters.")]
        public string? BirdName { get; set; }

        [Required]
        [DisplayName("Bird Information")]
        [StringLength(500, MinimumLength = 3)]
        //[RegularExpression(@"^(?<startString>\S+).*", ErrorMessage = "Bird information should start with a string.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\.\-\,]+$", ErrorMessage = "Bird information should only contain alphanumeric characters, spaces, periods, commas, and hyphens.")]
        public string? BirdInformation { get; set; }

        [DisplayName("File Name")]
        public string? FileName { get; set; }

        [Required]
        [NotMapped]
        [DisplayName("Bird Image")]
        public IFormFile? BirdImage { get; set; }
    }
}
