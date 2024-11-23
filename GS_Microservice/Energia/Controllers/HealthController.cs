using Microsoft.AspNetCore.Mvc;

namespace Energia.Controllers
{
    [ApiController]
    [Route("api/health")]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> _logger;

        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetHealthStatus()
        {
            try
            {

                var healthInfo = new
                {
                    Status = "OK",
                    RedisConnected = CheckRedisConnection(),
                    MongoConnected = CheckMongoConnection(),
                    Uptime = $"{Environment.TickCount64 / 1000 / 60} minutes",
                    Timestamp = DateTime.Now
                };

                _logger.LogInformation("Health check performed successfully.");
                return Ok(healthInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError("Health check failed: {Error}", ex.Message);
                return StatusCode(500, new { mensagem = "Erro ao verificar saúde do sistema.", erro = ex.Message });
            }
        }


        private bool CheckRedisConnection()
        {

            return true;
        }

        private bool CheckMongoConnection()
        {

            return true;
        }
    }
}