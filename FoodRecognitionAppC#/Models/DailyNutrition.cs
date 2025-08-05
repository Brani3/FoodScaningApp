using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodRecognitionAppC_.Models
{
    public class DailyNutrition
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public float Protein { get; set; }
        public float Carbs { get; set; }
        public float Fats { get; set; }
        public float Calories { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}

