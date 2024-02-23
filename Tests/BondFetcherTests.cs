using FluentAssertions;
using GuerrillaNtp;
using Microsoft.Extensions.Configuration;
using Infrastructure.Fetcher;
using Infrastructure.Options;
using Microsoft.Extensions.Options;


namespace Tests;

public class BondFetcherTests
{
    private readonly IConfiguration _testConfig;
    private readonly NtpClock _ntpClock;
    private readonly FetcherOptions _fetcherOptions;
    private readonly BondFetcher _bondFetcher;
    public BondFetcherTests()
    {
        // configuraton
        NtpClient client = NtpClient.Default;
        _ntpClock = client.Query();


        _testConfig = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings-test.json")
            .Build();

        _fetcherOptions = new FetcherOptions { Base = _testConfig.GetValue<string>("Fetching:Base") };
        var fetcherOptionsWrapper = Options.Create(_fetcherOptions);
        _bondFetcher = new(_ntpClock, fetcherOptionsWrapper);


    }

    [Theory]
    [InlineData("BOT")]
    [InlineData("BTP")]
    public async Task ExtractBondUrls_ShouldSucceed(string typeOfBond)
    {
        //arrange
        //act
        var bonds = await _bondFetcher.FetchBondsAsync(typeOfBond);

        //assert
        foreach (var bond in bonds)
        {
            bond.Name.Should().Contain($"{typeOfBond[0].ToString().ToUpper()}{typeOfBond.Substring(1).ToLower()}");
        }
        bonds.Should().HaveCountGreaterThan(5);
    }
}