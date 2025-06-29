using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EnglishDictionaryWeb.Models

{
    public class Word
    {
        public int Id { get; set; }

        [Required]
        public string Text { get; set; } // İngilizcesi

        [Required]
        public string Translation { get; set; } // Türkçesi

        public int Level { get; set; } = 3; // Bilinmeme seviyesi, başlangıçta 3

        [ForeignKey("AppUser")]
        public string AppUserId { get; set; } // Hangi kullanıcıya ait

        public AppUser AppUser { get; set; }
    }
}
