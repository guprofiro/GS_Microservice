using Domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StackExchange.Redis;
using Repositorios.Interfaces;

namespace Energia.Controllers
{
	[Route("api")]
	[ApiController]
	public class EnergiaController : ControllerBase
	{
		private static ConnectionMultiplexer redis;
		private readonly IEnergiaRepository _repository;

		public EnergiaController(IEnergiaRepository repository)
		{
			_repository = repository;
		}

		[HttpGet("health")]
		public async Task<IActionResult> GetEnergia()
		{
			string key = "getenergia";
			redis = ConnectionMultiplexer.Connect("localhost:6379");
			IDatabase db = redis.GetDatabase();

			try
			{
				await db.KeyExpireAsync(key, TimeSpan.FromSeconds(10));
				string user = await db.StringGetAsync(key);

				if (!string.IsNullOrEmpty(user))
				{
					return Ok(user);
				}

				var energias = await _repository.ObterTodosRegistros();

				if (energias == null)
				{
					return NotFound();
				}

				string energiasJson = JsonConvert.SerializeObject(energias);
				await db.StringSetAsync(key, energiasJson);

				return Ok(energias);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { mensagem = "Erro no servidor." });
			}
		}

		[HttpPost("energia")]
		public async Task<IActionResult> PostEnergia([FromBody] Energia energia)
		{
			if (energia == null)
			{
				return BadRequest(new { mensagem = "Dados inválidos. " });
			}

			try
			{
				await _repository.InserirRegistro(energia);

				string key = "getenergia";
				redis = ConnectionMultiplexer.Connect("localhost:6379");
				IDatabase db = redis.GetDatabase();
				await db.KeyDeleteAsync(key);

				return CreatedAtAction(nameof(GetEnergia), new { id = energia.Id }, new { mensagem = "Energia registrada com sucesso!" });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { mensagem = "Erro no servidor." });
			}
		}
	}
}
