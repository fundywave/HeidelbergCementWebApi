using HeidelbergCement.Data.DTO;
using HeidelbergCement.Service.Interface;
using HeidelbergCement.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace HeidelbergCement.Tests.Controller
{
    [TestFixture]
    public class LogProxyControllerTests
    {
        Mock<IConfiguration> _configurationMock;
        Mock<IAirTableDataProvider> _airTableDataProviderMock;
        LogProxyController _controller;
        [SetUp]
        public void Setup()
        {
            _configurationMock = new Mock<IConfiguration>();
            _airTableDataProviderMock = new Mock<IAirTableDataProvider>();
            _controller = new LogProxyController(_airTableDataProviderMock.Object, _configurationMock.Object);
        }
        [TearDown]
        public void TearDown()
        {
            _configurationMock.Reset();
            _airTableDataProviderMock.Reset();
        }
        [Test]
        public void Should_GetMessage_Return_Correct_Records()
        {
            //arrange
            var responseRecords = new ResponseRecord
            {
                records = new List<RecordDTO>(){
                    new RecordDTO
                    {
                        id = "1",
                        fields =new FieldDTO()
                        {
                            id="1",
                            Message="Hello",
                            Summary="Some text",
                            receivedAt=DateTime.Today.ToString()
                        }

                    } ,
                    new RecordDTO
                    {
                        id = "2",
                        fields =new FieldDTO()
                        {
                            id="1",
                            Message="Hello",
                            Summary="Some text",
                            receivedAt=DateTime.Today.ToString()
                        }

                    }
                }
            };
            _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).GetSection(It.IsAny<string>()).Value).Returns("url").Verifiable();
            _airTableDataProviderMock.Setup(x => x.GetRecords(It.IsAny<string>())).Returns(responseRecords).Verifiable();

            //act
            var expectedRecords = _controller.GetRecord();
            var OkResult = expectedRecords as OkObjectResult;
            //assert
            Assert.IsNotNull(OkResult);
            Assert.AreEqual(200,OkResult.StatusCode);
            Assert.IsNotNull(OkResult.Value);
            Assert.AreEqual(2, (OkResult.Value as ResponseRecord).records.Count);

        }
        [Test]
        public void Should_GetMessage_Return_BadRequest_If_URl_Is_Missing()
        {
            //arrange
            _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).GetSection(It.IsAny<string>()).Value).Returns(string.Empty).Verifiable();

            //act
            var expectedRecords = _controller.GetRecord();
            var badResult = expectedRecords as BadRequestObjectResult;

            //assert

            Assert.AreEqual(400, badResult.StatusCode);
            Assert.AreEqual("Url is missing!", badResult.Value);

        }
        [Test]
        public void Should_PostMessage_Return_Correct_Records()
        {
            //arrange
            var requestRecord = new RequestRecord
            {
                records = new List<RecordDTO>(){
                    new RecordDTO
                    {
                        id = "1",
                        fields =new FieldDTO()
                        {
                            id="1",
                            Message="Hello",
                            Summary="Some text",
                            receivedAt=DateTime.Today.ToString()
                        }

                    } ,
                    new RecordDTO
                    {
                        id = "2",
                        fields =new FieldDTO()
                        {
                            id="1",
                            Message="Hello",
                            Summary="Some text",
                            receivedAt=DateTime.Today.ToString()
                        }

                    }
                }
            };
            var responseRecords = new ResponseRecord
            {
                records = new List<RecordDTO>(){
                    new RecordDTO
                    {
                        id = "1",
                        fields =new FieldDTO()
                        {
                            id="1",
                            Message="Hello",
                            Summary="Some text",
                            receivedAt=DateTime.Today.ToString()
                        }

                    } ,
                    new RecordDTO
                    {
                        id = "2",
                        fields =new FieldDTO()
                        {
                            id="1",
                            Message="Hello",
                            Summary="Some text",
                            receivedAt=DateTime.Today.ToString()
                        }

                    }
                }
            };
            _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).GetSection(It.IsAny<string>()).Value).Returns("url").Verifiable();
            _airTableDataProviderMock.Setup(x => x.PostRecord(It.IsAny<string>(),requestRecord)).Returns(responseRecords).Verifiable();

            //act
            var expectedRecords = _controller.PostRecord(requestRecord);
            var OkResult = expectedRecords as OkObjectResult;

            //assert
            Assert.IsNotNull(OkResult);
            Assert.AreEqual(200, OkResult.StatusCode);
            Assert.IsNotNull(OkResult.Value);
            Assert.AreEqual(2, (OkResult.Value as ResponseRecord).records.Count);

        }
        [Test]
        public void Shold_PostMessage_Return_BadRequest_If_Model_Is_Invalid()
        {
            //arrange
            RequestRecord requestRecord = null;

            //act
            var expectedResult = _controller.PostRecord(requestRecord);
            var badResult = expectedResult as BadRequestObjectResult;
            //assert
            Assert.IsNotNull(badResult);
            Assert.AreEqual(400, badResult.StatusCode);
        }
        [Test]
        public void Shold_PostMessage_Return_BadRequest_If_Input_Is_Incorrect()
        {
            //arrange
            RequestRecord requestRecord = new RequestRecord();

            //act
            var expectedResult = _controller.PostRecord(requestRecord);
            var badResult = expectedResult as BadRequestObjectResult;
            //assert
            Assert.IsNotNull(badResult);
            Assert.AreEqual(400, badResult.StatusCode);
            Assert.AreEqual("The input format is incorrect!", badResult.Value);
            
        }
        [Test]
        public void Should_PostMessage_Return_BadRequest_If_URl_Is_Missing()
        {
            //arrange
            var requestRecord = new RequestRecord
            {
                records = new List<RecordDTO>(){
                    new RecordDTO
                    {
                        id = "1",
                        fields =new FieldDTO()
                        {
                            id="1",
                            Message="Hello",
                            Summary="Some text",
                            receivedAt=DateTime.Today.ToString()
                        }

                    } ,
                    new RecordDTO
                    {
                        id = "2",
                        fields =new FieldDTO()
                        {
                            id="1",
                            Message="Hello",
                            Summary="Some text",
                            receivedAt=DateTime.Today.ToString()
                        }

                    }
                }
            };
            _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).GetSection(It.IsAny<string>()).Value).Returns(string.Empty).Verifiable();

            //act
            var expectedRecords = _controller.PostRecord(requestRecord);
            var badResult = expectedRecords as BadRequestObjectResult;

            //assert

            Assert.AreEqual(400, badResult.StatusCode);
            Assert.AreEqual("Url is missing!", badResult.Value);

        }
    }
}
