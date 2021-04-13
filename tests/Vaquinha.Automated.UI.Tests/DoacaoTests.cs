using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Vaquinha.Tests.Common.Fixtures;
using Xunit;

namespace Vaquinha.AutomatedUITests
{
	public class DoacaoTests : IDisposable, IClassFixture<DoacaoFixture>,
											   IClassFixture<EnderecoFixture>,
											   IClassFixture<CartaoCreditoFixture>
	{
		private DriverFactory _driverFactory = new DriverFactory();
		private IWebDriver _driver;

		private readonly DoacaoFixture _doacaoFixture;
		private readonly EnderecoFixture _enderecoFixture;
		private readonly CartaoCreditoFixture _cartaoCreditoFixture;

		public DoacaoTests(DoacaoFixture doacaoFixture, EnderecoFixture enderecoFixture, CartaoCreditoFixture cartaoCreditoFixture)
		{
			_doacaoFixture = doacaoFixture;
			_enderecoFixture = enderecoFixture;
			_cartaoCreditoFixture = cartaoCreditoFixture;
		}
		public void Dispose()
		{
			_driverFactory.Close();
		}

		[Fact]
		public void DoacaoUI_AcessoTelaHome()
		{
			// Arrange
			_driverFactory.NavigateToUrl("https://localhost:44317/");
			_driver = _driverFactory.GetWebDriver();

			// Act
			IWebElement webElement = null;
			webElement = _driver.FindElement(By.ClassName("vaquinha-logo"));

			// Assert
			webElement.Displayed.Should().BeTrue(because: "logo exibido");
		}
		[Fact]
		public void DoacaoUI_CriacaoDoacao()
		{
			//Arrange
			var doacao = _doacaoFixture.DoacaoValida();
			doacao.AdicionarEnderecoCobranca(_enderecoFixture.EnderecoValido());
			doacao.AdicionarFormaPagamento(_cartaoCreditoFixture.CartaoCreditoValido());
			_driverFactory.NavigateToUrl("https://localhost:44317");
			_driver = _driverFactory.GetWebDriver();

			//Act
			IWebElement webElement = null;
			webElement = _driver.FindElement(By.ClassName("btn-yellow"));
			webElement.Click();

			IWebElement campoNome = _driver.FindElement(By.Id("DadosPessoais_Nome"));
			campoNome.SendKeys(doacao.DadosPessoais.Nome);


			IWebElement campoEmail = _driver.FindElement(By.Id("DadosPessoais_Email"));
			campoEmail.SendKeys(doacao.DadosPessoais.Email);

			IWebElement campoTexto = _driver.FindElement(By.Id("EndereçoCobranca_TextoEndereco"));
			campoTexto.SendKeys(doacao.EnderecoCobranca.TextoEndereco);

			IWebElement campoNumero = _driver.FindElement(By.Id("EndereçoCobranca_Numero"));
			campoNumero.SendKeys(doacao.EnderecoCobranca.Numero);

			IWebElement campoCidade = _driver.FindElement(By.Id("EndereçoCobranca_Cidade"));
			campoCidade.SendKeys(doacao.EnderecoCobranca.Cidade);

			IWebElement campoEstado = _driver.FindElement(By.Id("estado"));
			campoEstado.SendKeys(doacao.EnderecoCobranca.Estado);

			IWebElement campoCEP = _driver.FindElement(By.Id("cep"));
			campoCEP.SendKeys(doacao.EnderecoCobranca.CEP);

			IWebElement campoComplemento = _driver.FindElement(By.Id("EnderecoCobranca_Complemento"));
			campoComplemento.SendKeys(doacao.EnderecoCobranca.Complemento);

			IWebElement campoTelefone = _driver.FindElement(By.Id("telefone"));
			campoTelefone.SendKeys(doacao.EnderecoCobranca.Telefone);



			IWebElement cartaoNome = _driver.FindElement(By.Id("FormaPagamento_NomeTitular"));
			cartaoNome.SendKeys(doacao.FormaPagamento.NomeTitular);

			IWebElement cartaoNumero = _driver.FindElement(By.Id("cardNumber"));
			cartaoNumero.SendKeys(doacao.FormaPagamento.NumeroCartaoCredito);

			IWebElement cartaoData = _driver.FindElement(By.Id("validade"));
			cartaoData.SendKeys(doacao.FormaPagamento.Validade);

			IWebElement cartaoCVV = _driver.FindElement(By.Id("id"));
			cartaoCVV.SendKeys(doacao.FormaPagamento.CVV);

			webElement = _driver.FindElement(By.ClassName("btn-yellow"));
			webElement.Click();

			//Assert
			_driver.Url.Should().Contain("/Home/Index");
		}

	}
}