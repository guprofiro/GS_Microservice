using Moq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repositorios.Interfaces;
using Domain;
using Energia.Controllers;

namespace Teste
{
    public class EnergiaControllerTest
    {
        private readonly Mock<IEnergiaRepository> _energiaRepositoryMock;
        private readonly EnergiaController _controller;

        public EnergiaControllerTest()
        {
            _energiaRepositoryMock = new Mock<IEnergiaRepository>();
            _controller = new EnergiaController(_energiaRepositoryMock.Object);
        }

        [Fact]
        public async Task Get_ListarEnergiaOk()
        {
            //arrange
            var energias = new List<Energia>()
            {
                new Energia()
                {
                    Id = 1,
                    Status = "Normal",
                    ConsumoKwh = 1200.0,
                    DataMedicao = "21/11/2024",
                    LocalMedicao = "São Paulo, SP"
                }
            };
            _energiaRepositoryMock.Setup(c => c.ObterTodosRegistros()).ReturnsAsync(energias);

            //act
            var result = await _controller.GetEnergia();

            //assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(JsonConvert.SerializeObject(energias), JsonConvert.SerializeObject(okResult.Value));
        }

        [Fact]
        public async Task Get_ListarEnergiaNotFound()
        {
            //arrange
            _energiaRepositoryMock.Setup(c => c.ObterTodosRegistros()).ReturnsAsync((IEnumerable<Energia>)null);

            //act
            var result = await _controller.GetEnergia();

            //assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Post_SalvarEnergia()
        {
            //arrange
            var energia = new Energia()
            {
                Id = 1,
                Status = "Normal",
                ConsumoKwh = 1200.0,
                DataMedicao = "21/11/2024",
                LocalMedicao = "São Paulo, SP"
            };
            _energiaRepositoryMock.Setup(c => c.InserirRegistro(It.IsAny<Energia>())).Returns(Task.CompletedTask);

            //act
            var result = await _controller.PostEnergia(energia);

            //assert
            Assert.IsType<OkObjectResult>(result);
            _energiaRepositoryMock.Verify(c => c.InserirRegistro(It.IsAny<Energia>()), Times.Once);
        }

    }
}
