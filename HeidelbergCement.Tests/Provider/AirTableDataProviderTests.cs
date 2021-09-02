using HeidelbergCement.Data.DTO;
using HeidelbergCement.Service.Interface;
using HeidelbergCement.Service.Provider;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;


namespace HeidelbergCement.Tests.Provider
{
    [TestFixture]
    public class AirTableDataProviderTests
    {
        Mock<IDataProvider<ResponseRecord>> _dataProviderMock;
        IAirTableDataProvider _airTableDataProvider;
        [SetUp]
        public void Setup()
        {
            _dataProviderMock = new Mock<IDataProvider<ResponseRecord>>();
            _airTableDataProvider = new AirTableDataProvider(_dataProviderMock.Object);
        }
        [TearDown]
        public void TearDown()
        {
            _dataProviderMock.Reset();
        }
        [Test]
        public void Should_GetRecords_Return_AirTable_Records()
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
            _dataProviderMock.Setup(x => x.Get(string.Empty)).Returns(responseRecords);

            //act
            var expectedRecords = _airTableDataProvider.GetRecords(string.Empty);

            //assert
            Assert.IsNotNull(expectedRecords);
            Assert.AreEqual(2, expectedRecords.records.Count);
            _dataProviderMock.Verify(x => x.Get(string.Empty), Times.Once);

        }

        [Test]
        public void Should_PostRecord_Return_AirTable_Record()
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
                            receivedAt="01.01.2000"
                        }

                    }

                }
            };

            var records = requestRecord.records[0].fields;
            RequestRecord insertedRecord = null;

            _dataProviderMock.Setup(x => x.Post(It.IsAny<string>(), requestRecord)).Callback<string, RequestRecord>((url, request) => insertedRecord = request).Returns(new ResponseRecord
                {
                    records = new List<RecordDTO>(){
                    new RecordDTO
                    {
                        id = requestRecord.records[0].id,
                        fields =new FieldDTO()
                        {
                            id=records.id,
                            Message=records.Message,
                            Summary=records.Summary,
                            receivedAt=records.receivedAt
                        }

                    }
                    }

                }).Verifiable();

            //act
            var expectedRecords = _airTableDataProvider.PostRecord(string.Empty, requestRecord);

            //assert
            Assert.IsNotNull(expectedRecords);
            Assert.AreEqual(1,expectedRecords.records.Count);
            Assert.AreEqual(insertedRecord.records[0].id, expectedRecords.records[0].id);
            Assert.AreEqual(insertedRecord.records[0].fields.id, expectedRecords.records[0].fields.id);
            Assert.AreEqual(insertedRecord.records[0].fields.Message, expectedRecords.records[0].fields.Message);
            Assert.AreEqual(insertedRecord.records[0].fields.receivedAt, expectedRecords.records[0].fields.receivedAt);
            Assert.AreEqual(insertedRecord.records[0].fields.Summary, expectedRecords.records[0].fields.Summary);

        }
    }

}
