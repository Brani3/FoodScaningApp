using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodRecognitionAppC_.Data;
using FoodRecognitionAppC_.DTO;
using FoodRecognitionAppC_.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FoodRecognitionAppC_.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NutritionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NutritionController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int GetUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null) throw new Exception("User ID claim not found");
            return int.Parse(userIdClaim.Value);
        }

        [HttpPost("log-food")]
        public async Task<IActionResult> LogFood([FromBody] LogFoodDto dto)
        {
            int userId = GetUserId();

            var today = DateTime.UtcNow.Date;

            var entry = await _context.DailyNutrition
                .FirstOrDefaultAsync(d => d.UserId == userId && d.Date == today);

            if (entry == null)
            {
                entry = new DailyNutrition
                {
                    UserId = userId,
                    Date = today,
                    Protein = dto.Protein,
                    Carbs = dto.Carbs,
                    Fats = dto.Fats,
                    Calories = dto.Calories
                };
                _context.DailyNutrition.Add(entry);
            }
            else
            {
                entry.Protein += dto.Protein;
                entry.Carbs += dto.Carbs;
                entry.Fats += dto.Fats;
                entry.Calories += dto.Calories;

                _context.DailyNutrition.Update(entry);
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Food logged successfully." });
        }
    }
}

