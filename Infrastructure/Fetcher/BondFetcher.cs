using Application.Interfaces;
using Domain.Entities;
using Domain.Exeptions;
using GuerrillaNtp;
using HtmlAgilityPack;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Infrastructure.Fetcher;

/// <summary>
/// Represents a service for fetching bonds.
/// </summary>
public class BondFetcher : IBondFetcher
{
    private readonly NtpClock _ntpClock;
    private readonly FetcherOptions _fetcherOptions;

    public BondFetcher(NtpClock ntpClock, IOptions<FetcherOptions> fetcherOptions)
    {
        _ntpClock = ntpClock;
        _fetcherOptions = fetcherOptions.Value;
    }
    /// <inheritdoc />
    public async Task<IEnumerable<BondSnapshot>> FetchBondsAsync(string typeOfBond)
    {
        string destination = _fetcherOptions.Base + typeOfBond;

        List<string> bondUrls = await ExtractBondUrlsAsync(destination);

        List<BondSnapshot> bonds = new List<BondSnapshot>();

        int batchSize = 15;

        for (int i = 0; i < bondUrls.Count; i += batchSize)
        {
            var batchUrls = bondUrls.Skip(i).Take(batchSize).ToList();

            await Task.WhenAll(batchUrls.Select(async bondUrl =>
            {
                try
                {
                    BondSnapshot bond = await Task.Run(() => ExtractBondSnap(bondUrl, typeOfBond));
                    bonds.Add(bond);
                }
                catch (FailedToParseExeption)
                {
                    Debug.WriteLine($"Failed to parse a mandatory element, skipping {bondUrl}");
                }
                catch (FailedToScrapeExeption)
                {

                    Debug.WriteLine($"Failed to find a mandatory element, skipping {bondUrl}");
                }
            }));
        }

        return bonds;
    }

    // PRIVATE METHODS
    private BondSnapshot ExtractBondSnap(string bondUrl, string typeOfBond)
    {
        DateTime utcDateTime = _ntpClock.UtcNow.UtcDateTime;
        string html = DownloadHtmlAsync(bondUrl).Result;
        HtmlDocument document = new();
        document.LoadHtml(html);

        string bondName = string.Empty;
        string isin = string.Empty;
        string stringPrice = string.Empty;
        string stringCedolaAnnuale = string.Empty;
        string stringRendimentoannuolordo = string.Empty;
        string stringScadenza = string.Empty;

        // extracting the values from the webpage in string format
        try
        {
            var headerDiv = document.DocumentNode.SelectSingleNode("//div[contains(@class, 'panel-header-scheda')]");
            bondName = headerDiv.SelectSingleNode("//h1").InnerText;
            isin = headerDiv.SelectSingleNode("//div[contains(@class,'header-market-info')]").InnerText;
            stringPrice = headerDiv.SelectSingleNode("//span[contains(@class,'price')]").InnerText;
            stringCedolaAnnuale = document.DocumentNode.SelectSingleNode("//li[text()='Cedola annuale']//span").InnerText;
            stringRendimentoannuolordo = document.DocumentNode.SelectSingleNode("//li[text()='Rendimento annuo lordo']//span").InnerText;
            stringScadenza = document.DocumentNode.SelectSingleNode("//li[text()='Scadenza']//span").InnerText;
        }
        catch (Exception)
        {
            throw new FailedToScrapeExeption($"Unable to find an element in \"{bondUrl}\"");
        }

        // parse mendatory values
        Debug.WriteLine($"Started parsing this: {bondName} {isin}");
        isin = ExtractISINCode(isin);
        double price = ParseDouble(stringPrice, true);
        double rendimentoannuolordo = ParseDouble(stringRendimentoannuolordo, true);

        string format = "dd/MM/yyyy";
        DateTime scadenza = DateTime.ParseExact(stringScadenza, format, CultureInfo.InvariantCulture);



        // parse non mendatory values
        double? cedolaAnnuale = ParseDouble(stringCedolaAnnuale);

        BondSnapshot bond = new(utcDateTime,
                        isin,
                        bondName,
                        cedolaAnnuale,
                        scadenza,
                        bondUrl,
                        typeOfBond,
                        price,
                        rendimentoannuolordo);
        return bond;

    }

    private double ParseDouble(string stringDouble, bool isMendatory = false)
    {
        stringDouble = stringDouble.Replace("%", "");
        stringDouble = stringDouble.Replace("(", "");
        stringDouble = stringDouble.Replace(")", "");
        double doubleValue;
        bool isParsed = double.TryParse(stringDouble, out doubleValue);
        if (isParsed)
        {
            return doubleValue;
        }
        else
        {
            if (isMendatory)
            {
                throw new FailedToParseExeption($"Unable to extract the double value from the string \"{stringDouble}\"");
            }
            else
            {
                return 0;
            }
        }
    }

    private async Task<List<string>> ExtractBondUrlsAsync(string baseUrl)
    {
        string html = await DownloadHtmlAsync(baseUrl);



        List<string> bondUrls = new();
        HtmlDocument document = new();
        document.LoadHtml(html);


        HtmlNodeCollection rows = document.DocumentNode.SelectNodes("//tr//td//a");

        if (rows != null)
        {
            foreach (var row in rows)
            {
                HtmlAttribute htmlAttribute = row.Attributes["href"];
                if (htmlAttribute != null)
                {
                    string absoluteUrl = new Uri(new Uri(baseUrl), htmlAttribute.Value).AbsoluteUri;
                    bondUrls.Add(absoluteUrl);
                }
            }
        }

        return bondUrls;
    }

    private async Task<string> DownloadHtmlAsync(string url)
    {
        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");
            var aaa = await httpClient.GetStringAsync(url);
            return aaa;
        }

    }


    static string ExtractISINCode(string input)
    {
        string pattern = @"ISIN:\s*([A-Z]{2}[A-Z0-9]{10})";
        Match match = Regex.Match(input, pattern);
        if (match.Success)
        {
            // Extract and return the ISIN code from the match
            return match.Groups[1].Value;
        }
        else
        {
            throw new FailedToParseExeption("Unable to parse ISIN");
        }
    }
}
