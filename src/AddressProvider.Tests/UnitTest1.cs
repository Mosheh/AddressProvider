using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AddressProvider.Test
{
    [TestClass]
    public class TestPostmon
    {
        [TestMethod]
        public void TestarRequisiçãoDeEndereçoPostmon()
        {
            var service = new AddressService(ServiceEnum.ViaCEP);
            var address = service.GetAddress("74223170");
            Assert.AreEqual("Rua T 61", address.Street);
        }

        [TestMethod]
        public void TestarRequisiçãoDeEndereçoViaCEP()
        {
            var service = new AddressService(ServiceEnum.ViaCEP);
            var address = service.GetAddress("74223-170");
            Assert.AreEqual("Rua T 61", address.Street);
        }

        [TestMethod]
        public void TestarRequisiçãoDeEndereçoIBGE()
        {
            var service = new AddressService(ServiceEnum.ViaCEP);
            var address = service.GetAddress("74223-170");
            var ibgeGoiania = "5208707";
            Assert.AreEqual(ibgeGoiania, address.CityInfo.IBGECode);
        }

        [TestMethod]
        public void TestarRemocaoDeCaracterEspecialDoCep()
        {
            var service = new AddressService(ServiceEnum.Postmon);
            var address = service.RemoveCaracter("74223-170");
            Assert.AreEqual(address, "74223170");

        }

        [TestMethod]
        public void TestarQuantidadeDeCaracter()
        {
            var service = new AddressService(ServiceEnum.Postmon);
            var isValid = service.ValidQuantityCaracter("74223-170");
            Assert.AreEqual(isValid, true);
        }

        [TestMethod]
        public void TestarRequisicaoDeEnderecoPorArea()
        {
            var service = new AddressService(ServiceEnum.ViaCEP);
            var address = service.GetAddress("74477-207");
            var bairro = "Conjunto Primavera";
            Assert.AreEqual(bairro, address.Neighborhood);

        }

        [TestMethod]
        public void TestarUFGOPorCEP()
        {
            var service = new AddressService(ServiceEnum.ViaCEP);
            var address = service.GetAddress("74477-207");
            var State = "GO";
            Assert.AreEqual(State, address.State);
        }

        [TestMethod]
        public void TestarConsultaComCepSemHífen()
        {
            var service = new AddressService(ServiceEnum.ViaCEP);
            var address = service.GetAddress("74477207");
            var State = "GO";
            Assert.AreEqual(State, address.State);
        }

        [TestMethod]
        public void TestarMensagemCEPInvalido()
        {
            var service = new AddressService(ServiceEnum.ViaCEP);
            try
            {
                var address = service.GetAddress("11111111");
            }
            catch (Exception ex) { throw ex; }
        }

        [TestMethod]
        public void TestarBairroPeloCEP()
        {
            var service = new AddressService(ServiceEnum.ViaCEP);
            var address = service.GetAddress("74922330");
            var Bairro = "Jardim Olímpico";
            Assert.AreEqual(Bairro, address.Neighborhood);
        }

        [TestMethod]
        public void TestarEnderecoEstadosUnidosNovaYork()
        {
            var service = new AddressService(ServiceEnum.TargetLock);
            var address = service.GetAddress("11742");
            var Bairro = "Suffolk";
            Assert.AreEqual(Bairro, address.Neighborhood);
        }

        [TestMethod]
        public void TestarBairroNullCEPGenerico()
        {
            var service = new AddressService(ServiceEnum.ViaCEP);
            var address = service.GetAddress("77500000");
            Assert.IsNotNull(address.Neighborhood);
        }

        [TestMethod]
        public void TestarBairroNaoNullo()
        {
            var service = new AddressService(ServiceEnum.ViaCEP);
            var address = service.GetAddress("74922330");
            Assert.IsNotNull(address.Neighborhood);
        }

        [TestMethod]
        public void TestarUF()
        {
            var service = new AddressService(ServiceEnum.ViaCEP);
            var address = service.GetAddress("74922330");
            var State = "TO";
            Assert.AreNotEqual(State, address.State);
        }


    }
}
