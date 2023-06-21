using Bogus;
using Bogus.Extensions.Brazil;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace SistemaContas.Tests
{
    public class ClienteCadastroTest
    {
        [Fact]
        public void CadastrarClienteComSucesso()
        {
            var driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl("http://localhost:4200/cadastro-clientes");

            var faker = new Faker("pt_BR");

            var nome = driver.FindElement(By.Id("nome"));
            nome.Clear();
            nome.SendKeys($"{faker.Name.FirstName()}");

            var cpf = driver.FindElement(By.Id("cpf"));
            cpf.Clear();
            cpf.SendKeys(string.Join("", faker.Random.Digits(11)));

            var email = driver.FindElement(By.Id("email"));
            email.Clear();
            email.SendKeys(faker.Person.Email);

            var dataNascimento = driver.FindElement(By.Id("dataNascimento"));
            dataNascimento.Clear();
            dataNascimento.SendKeys(faker.Person.DateOfBirth.ToShortDateString());

            var botao = driver.FindElement(By.CssSelector("input[type='submit']"));
            botao.Click();

            var mensagem = driver.FindElement(By.XPath("//div[@class='alert alert-success']"));

            var resultadoEsperado = "Sucesso! Cliente cadastrado com sucesso!";
            var resultadoObtido = mensagem.Text.Trim();

            Assert.Equal(resultadoEsperado, resultadoObtido);

            driver.Close();
            driver.Quit();
        }
    }
}