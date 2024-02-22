using AutoMapper;
using Dapper;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Mapping;
using Infrastructure.Options;
using Infrastructure.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Data;

namespace Tests
{
    public class BondRepositoryTests
    {
        private readonly IConfiguration _testConfig;
        private readonly DatabaseOptions _databaseOptions;
        private readonly IMapper _mapper;
        private readonly BondRepository _bondRepository;
        private readonly string _databaseConnectionString;


        public BondRepositoryTests()
        {
            // Mapper configuration
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            _mapper = mapperConfig.CreateMapper();

            // configuraton
            _testConfig = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings-test.json")
                .Build();

            _databaseOptions = new DatabaseOptions
            {
                Host = "localhost,11433",
                Port = _testConfig.GetValue<string>("DatabaseSettings:Port"),
                Password = _testConfig.GetValue<string>("DatabaseSettings:Password"),
                Userid = _testConfig.GetValue<string>("DatabaseSettings:Userid"),
                UsersDataBase = _testConfig.GetValue<string>("DatabaseSettings:UsersDataBase")

            };

            var databaseOptionsWrapper = Options.Create(_databaseOptions);
            _bondRepository = new(_mapper, databaseOptionsWrapper);

            _databaseConnectionString = $"Server={_databaseOptions.Host};Database={_databaseOptions.UsersDataBase};User={_databaseOptions.Userid};Password={_databaseOptions.Password};Encrypt=False";

        }

        private void ClearAllTestTables()
        {
            using (IDbConnection connection = new SqlConnection(_databaseConnectionString))
            {
                connection.Open();
                string query = @"
                    DELETE FROM BondsValues;
                    DELETE FROM Bonds;
                    ";
                connection.Execute(query);
            }
        }

        // tests

        [Fact]
        public void StoreBondSnapshots_ShouldSucceed()
        {
            // arrange
            ClearAllTestTables();

            BondSnapshot bondsnapshot1 = new(DateTime.Now, "111111", "111111", 1.1, DateTime.Now.AddMonths(2),
                                                "https://exampleurl.com", "BOT", 1.1, 1.1);

            BondSnapshot bondsnapshot2 = new(DateTime.Now, "222222", "222222", 2.2, DateTime.Now.AddMonths(2),
                                                "https://exampleurl.com", "BOT", 2.2, 2.2);

            BondSnapshot bondsnapshot3 = new(DateTime.Now, "333333", "333333", 3.3, DateTime.Now.AddMonths(2),
                                    "https://exampleurl.com", "BTP", 3.3, 3.3);

            List<BondSnapshot> BOTbondsnapshots = new();
            List<BondSnapshot> BTPbondsnapshots = new();

            BOTbondsnapshots.Add(bondsnapshot1);
            BOTbondsnapshots.Add(bondsnapshot2);
            BTPbondsnapshots.Add(bondsnapshot3);

            // act #1
            _bondRepository.StoreBondSnapshots(BOTbondsnapshots, "BOT");
            _bondRepository.StoreBondSnapshots(BTPbondsnapshots, "BTP");
            IList<Bond> activeBotsA = _bondRepository.GetActiveBondsAsync("BOT").Result;
            IList<Bond> activeBtpsA = _bondRepository.GetActiveBondsAsync("BTP").Result;

            // assert #1
            activeBotsA.Should().HaveCount(2);
            activeBotsA[0].Isin.Should().Be("111111");
            activeBotsA[0].YearlyCoupon.Should().Be(1.1);
            activeBotsA[1].Isin.Should().Be("222222");
            activeBotsA[1].YearlyCoupon.Should().Be(2.2);

            activeBtpsA.Should().HaveCount(1);
            activeBtpsA[0].Isin.Should().Be("333333");
            activeBtpsA[0].YearlyCoupon.Should().Be(3.3);

            // arrange #2
            BondSnapshot bondsnapshot1a = new(DateTime.Now, "222222", "222222", 2.2, DateTime.Now.AddMonths(2),
                                    "https://exampleurl.com", "BOT", 2.22, 2.22);

            List<BondSnapshot> BOTbondsnapshotsB = new();
            BOTbondsnapshotsB.Add(bondsnapshot1a);
            
            // act #2
            _bondRepository.StoreBondSnapshots(BOTbondsnapshotsB, "BOT");
            IList<Bond> activeBotsB = _bondRepository.GetActiveBondsAsync("BOT").Result;
            IList<Bond> activeBtpsB = _bondRepository.GetActiveBondsAsync("BTP").Result;

            // assert #2
            activeBotsB.Should().HaveCount(1);
            activeBotsB[0].Isin.Should().Be("222222");
            activeBotsB[0].YearlyCoupon.Should().Be(2.2);
            activeBotsB[0].HistoricalValues.Last().Price.Should().Be(2.22);

            activeBtpsB.Should().HaveCount(1);
            activeBtpsB[0].Isin.Should().Be("333333");
            activeBtpsB[0].YearlyCoupon.Should().Be(3.3);
            activeBtpsB[0].HistoricalValues.Last().Price.Should().Be(3.3);
        }
    }
}
