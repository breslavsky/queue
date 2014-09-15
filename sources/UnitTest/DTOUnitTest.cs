using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Queue.Model;
using Queue.Services.Server;
using DTO = Queue.Services.DTO;

namespace Queue.UnitTest
{
    [TestClass]
    public class DTOUnitTest
    {
        public DTOUnitTest()
        {
            Mapper.AddProfile(new FullDTOProfile());
        }

        [TestMethod]
        public void FullDTOProfile()
        {
            Mapper.Map<User, DTO.User>(new User() { });

            Mapper.Map<Workplace, DTO.Workplace>(new Workplace() { });

            Mapper.Map<DefaultConfig, DTO.DefaultConfig>(new DefaultConfig() { });

            Mapper.Map<Office, DTO.Office>(new Office() { });

            var clientRequest = Mapper.Map<ClientRequest, DTO.ClientRequest>(new ClientRequest());

            Assert.IsTrue(true);
        }
    }
}