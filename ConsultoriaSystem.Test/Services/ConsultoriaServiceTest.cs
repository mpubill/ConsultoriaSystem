using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsultoriaSystem.Api.Dtos;
using ConsultoriaSystem.Api.Entities;
using ConsultoriaSystem.Api.Repositories;
using ConsultoriaSystem.Api.Services;
using Moq;
using Xunit;

namespace ConsultoriaSystem.Test.Services
{
	public class ConsultoriaServiceTest
	{
		private readonly Mock<IConsultoresRepository> _repoMock;
		private readonly ConsultoresService _service;

		public ConsultoriaServiceTest()
		{
			_repoMock = new Mock<IConsultoresRepository>();
			_service = new ConsultoresService(_repoMock.Object);
		}

		// ----------------------------------------------------------------------
		// CREATE
		// ----------------------------------------------------------------------

		/// <summary>
		/// Este test valida el flujo completo de creación de un consultor.
		/// </summary>
		[Fact]
		public async Task CreateAsync_Deberia_InsertarConsultorYDevolverId()
		{
			// Arrange
			var dto = new ConsultorDTO
			{
				Nombre = "Raúl",
				AreaEspecializacion = "Backend",
				TarifaHora = 50,
				EmailCorporativo = "raul@empresa.com"
			};

			_repoMock
				.Setup(r => r.InsertAsync(It.IsAny<Consultor>()))
				.ReturnsAsync(10);

			// Act
			var result = await _service.CreateAsync(dto);

			// Assert
			Assert.Equal(10, result);

			_repoMock.Verify(r =>
				r.InsertAsync(It.Is<Consultor>(c =>
					c.Nombre == dto.Nombre &&
					c.AreaEspecializacion == dto.AreaEspecializacion &&
					c.TarifaHora == dto.TarifaHora &&
					c.EmailCorporativo == dto.EmailCorporativo &&
					c.Activo == true &&         // regla del negocio
					c.FechaIngreso != default   // debe asignarse fecha
				)),
				Times.Once);
		}

		// ----------------------------------------------------------------------
		// UPDATE
		// ----------------------------------------------------------------------

		[Fact]
		public async Task UpdateAsync_Deberia_LanzarArgumentException_SiConsultorIdEsNull()
		{
			// Arrange
			var dto = new ConsultorDTO
			{
				ConsultorId = null,
				Nombre = "Test",
				AreaEspecializacion = "Test",
				TarifaHora = 100,
				EmailCorporativo = "test@empresa.com"
			};

			// Act & Assert
			var ex = await Assert.ThrowsAsync<ArgumentException>(
				() => _service.UpdateAsync(dto)
			);

			Assert.Equal("ConsultorId es requerido para actualizar.", ex.Message);

			_repoMock.Verify(r => r.UpdateAsync(It.IsAny<Consultor>()), Times.Never);
		}

		[Fact]
		public async Task UpdateAsync_Deberia_UsarActivoTrue_CuandoDtoActivoEsNull()
		{
			// Arrange
			var dto = new ConsultorDTO
			{
				ConsultorId = 1,
				Nombre = "Raúl",
				AreaEspecializacion = "Backend",
				TarifaHora = 60,
				EmailCorporativo = "raul@empresa.com",
				Activo = null
			};

			_repoMock
				.Setup(r => r.UpdateAsync(It.IsAny<Consultor>()))
				.Returns(Task.CompletedTask);

			// Act
			await _service.UpdateAsync(dto);

			// Assert
			_repoMock.Verify(r =>
				r.UpdateAsync(It.Is<Consultor>(c =>
					c.ConsultorId == dto.ConsultorId &&
					c.Activo == true  // lógica por defecto
				)),
				Times.Once);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public async Task UpdateAsync_Deberia_RespetarValorDeActivo(bool activo)
		{
			// Arrange
			var dto = new ConsultorDTO
			{
				ConsultorId = 2,
				Nombre = "Otro",
				AreaEspecializacion = "Frontend",
				TarifaHora = 70,
				EmailCorporativo = "otro@empresa.com",
				Activo = activo
			};

			_repoMock
				.Setup(r => r.UpdateAsync(It.IsAny<Consultor>()))
				.Returns(Task.CompletedTask);

			// Act
			await _service.UpdateAsync(dto);

			// Assert
			_repoMock.Verify(r =>
				r.UpdateAsync(It.Is<Consultor>(c =>
					c.ConsultorId == dto.ConsultorId &&
					c.Activo == activo
				)),
				Times.Once);
		}

		// ----------------------------------------------------------------------
		// DELETE
		// ----------------------------------------------------------------------

		[Fact]
		public async Task DeleteAsync_Deberia_DevolverTrue_CuandoRepoDevuelveFilasMayoresACero()
		{
			// Arrange
			int id = 1;
			_repoMock
				.Setup(r => r.DeleteAsync(id))
				.ReturnsAsync(1);

			// Act
			var result = await _service.DeleteAsync(id);

			// Assert
			Assert.True(result);
			_repoMock.Verify(r => r.DeleteAsync(id), Times.Once);
		}

		[Fact]
		public async Task DeleteAsync_Deberia_DevolverFalse_CuandoRepoDevuelveCeroFilas()
		{
			// Arrange
			int id = 1;
			_repoMock
				.Setup(r => r.DeleteAsync(id))
				.ReturnsAsync(0);

			// Act
			var result = await _service.DeleteAsync(id);

			// Assert
			Assert.False(result);
			_repoMock.Verify(r => r.DeleteAsync(id), Times.Once);
		}

		// ----------------------------------------------------------------------
		// GET BY ID
		// ----------------------------------------------------------------------

		[Fact]
		public async Task GetByIdAsync_Deberia_DevolverNull_SiConsultorNoExiste()
		{
			// Arrange
			int id = 1;
			_repoMock
				.Setup(r => r.GetByIdAsync(id))
				.ReturnsAsync((Consultor?)null);

			// Act
			var result = await _service.GetByIdAsync(id);

			// Assert
			Assert.Null(result);
		}

		
		[Fact]
		public async Task GetByIdAsync_Deberia_DevolverDtoMapeado_CuandoConsultorExiste()
		{
			// Arrange
			int id = 1;
			var entity = new Consultor
			{
				ConsultorId = id,
				Nombre = "Raúl",
				AreaEspecializacion = "Backend",
				TarifaHora = 80,
				EmailCorporativo = "raul@empresa.com",
				Activo = true,
				FechaIngreso = new DateTime(2024, 1, 1)
			};

			_repoMock
				.Setup(r => r.GetByIdAsync(id))
				.ReturnsAsync(entity);

			// Act
			var result = await _service.GetByIdAsync(id);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(entity.ConsultorId, result!.ConsultorId);
			Assert.Equal(entity.Nombre, result.Nombre);
			Assert.Equal(entity.AreaEspecializacion, result.AreaEspecializacion);
			Assert.Equal(entity.TarifaHora, result.TarifaHora);
			Assert.Equal(entity.EmailCorporativo, result.EmailCorporativo);
			Assert.Equal(entity.Activo, result.Activo);
			Assert.Equal(entity.FechaIngreso, result.FechaIngreso);
		}

		// ----------------------------------------------------------------------
		// GET ALL
		// ----------------------------------------------------------------------

		[Fact]
		public async Task GetAllAsync_Deberia_DevolverListaDeDtosMapeados()
		{
			// Arrange
			var entities = new List<Consultor>
			{
				new Consultor
				{
					ConsultorId = 1,
					Nombre = "A",
					AreaEspecializacion = "Area A",
					TarifaHora = 50,
					EmailCorporativo = "a@empresa.com",
					Activo = true,
					FechaIngreso = DateTime.UtcNow.AddDays(-10)
				},
				new Consultor
				{
					ConsultorId = 2,
					Nombre = "B",
					AreaEspecializacion = "Area B",
					TarifaHora = 60,
					EmailCorporativo = "b@empresa.com",
					Activo = false,
					FechaIngreso = DateTime.UtcNow.AddDays(-5)
				}
			};

			_repoMock
				.Setup(r => r.GetAllAsync())
				.ReturnsAsync(entities);

			// Act
			var result = await _service.GetAllAsync();

			// Assert
			var list = result.ToList();
			Assert.Equal(2, list.Count);
			Assert.Equal(entities[0].ConsultorId, list[0].ConsultorId);
			Assert.Equal(entities[1].ConsultorId, list[1].ConsultorId);
		}
	}
}

