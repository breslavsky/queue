using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTest.DTO
{
    public class MainObject
    {
        public string Name { get; set; }

        public SubObject[] Collection { get; set; }
    }

    public class MainObjectAnother
    {
        public string Name { get; set; }
    }

    public class SubObject
    {
        public string Name { get; set; }

        public FinishObject[] Collection { get; set; }
    }

    public class FinishObject
    {
        public string Name { get; set; }
    }
}

namespace UnitTest
{
    public class MainObject
    {
        public string Name { get; set; }

        public List<SubObject> Collection { get; set; }
    }

    public class SubObject
    {
        public string Name { get; set; }

        public List<FinishObject> Collection { get; set; }
    }

    public class FinishObject
    {
        public string Name { get; set; }
    }

    [TestClass]
    public class AutomapperTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Mapper.CreateMap<MainObject, DTO.MainObject>();
            Mapper.CreateMap<SubObject, DTO.SubObject>();
            Mapper.CreateMap<FinishObject, DTO.FinishObject>();

            Mapper.CreateMap<MainObject, DTO.MainObjectAnother>();

            var mainObject = new MainObject()
            {
                Name = "Главный объект",
                Collection = new List<SubObject>()
                {
                    new SubObject(){
                        Name = "Вложенный объект 1",
                        Collection = new List<FinishObject>(){
                            new FinishObject(){
                                Name = "Конечный объект 1"
                            },
                            new FinishObject(){
                                Name = "Конечный объект 1"
                            }
                        }
                    },
                    new SubObject(){
                        Name = "Вложенный объект 2",
                        Collection = new List<FinishObject>(){
                            new FinishObject(){
                                Name = "Конечный объект 3"
                            },
                            new FinishObject(){
                                Name = "Конечный объект 4"
                            }
                        }
                    }
                }
            };

            DTO.MainObjectAnother dto = Mapper.Map<MainObject, DTO.MainObjectAnother>(mainObject);

            string name = dto.Name;
        }
    }
}