using Domain;
using Repositorios.Interfaces;
using Moq;
using Xunit;

namespace Teste
{
    public class EnergiaRepositoryTest
    {
        [Fact]
        public async Task ObterTodosRegistros_DeveRetornarTodosOsRegistros()
        {
            var registrosMock = new List<Energia>()
            {
                new Energia()
                {
                    Id = 1,
                    Status = "Normal",
                    ConsumoKwh = 1500.0,
                    DataMedicao = "22/11/2024",
                    LocalMedicao = "Rio de Janeiro, RJ"
                },
                new Energia()
                {
                    Id = 2,
                    Status = "Alto",
                    ConsumoKwh = 7500.0,
                    DataMedicao = "21/11/2024",
                    LocalMedicao = "Rio de Janeiro, RJ"
                }
            };

            var energiaRepositoryMock = new Mock<IEnergiaRepository>();
            energiaRepositoryMock.Setup(r => r.ObterTodosRegistros()).ReturnsAsync(registrosMock);
            var energiaRepository = energiaRepositoryMock.Object;

            var resultado = await energiaRepository.ObterTodosRegistros();

            Assert.Equal(registrosMock, resultado);
        }

        [Fact]
        public async Task InserirRegistro_DeveSalvarNovoRegistro()
        {
            var novoRegistro = new Energia()
            {
                Id = 3,
                Status = "Baixo",
                ConsumoKwh = 900.0,
                DataMedicao = "23/11/2024",
                LocalMedicao = "Belo Horizonte, MG"
            };

            var energiaRepositoryMock = new Mock<IEnergiaRepository>();
            energiaRepositoryMock.Setup(r => r.InserirRegistro(novoRegistro)).Returns(Task.CompletedTask);
            var energiaRepository = energiaRepositoryMock.Object;

            await energiaRepository.InserirRegistro(novoRegistro);

            energiaRepositoryMock.Verify(r => r.InserirRegistro(novoRegistro), Times.Once);
            Assert.True(true);
        }
    }
}
