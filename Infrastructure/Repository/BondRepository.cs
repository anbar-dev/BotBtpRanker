using Application.Interfaces;
using AutoMapper;
using Dapper;
using Domain.Entities;
using Infrastructure.Options;
using Infrastructure.Repository.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace Infrastructure.Repository;

public class BondRepository : IBondRepository
{
    private readonly string _cointainerConnectionString;
    private readonly string _databaseConnectionString;
    private readonly IMapper _mapper;
    private readonly DatabaseOptions _databaseOptions;


    public BondRepository(IMapper mapper, IOptions<DatabaseOptions> options)
    {
        _mapper = mapper;
        _databaseOptions = options.Value;
        // Please note the field Encrypt=False. This is a really ugly and dangerous workaround needed in dev env so that we can connect to the database without a proper certificate
        _cointainerConnectionString = $"Server={_databaseOptions.Host};User={_databaseOptions.Userid};Password={_databaseOptions.Password};Encrypt=False";
        _databaseConnectionString = $"Server={_databaseOptions.Host};Database={_databaseOptions.UsersDataBase};User={_databaseOptions.Userid};Password={_databaseOptions.Password};Encrypt=False";
    }


    // PUBLIC METHODS (INTERFACE)
    public void CreateDatabaseIfNotExists()
    {
        using (IDbConnection connection = new SqlConnection(_cointainerConnectionString))
        {
            connection.Open();
            string query = @"

                IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'BotBtpDB') 
                BEGIN 
                CREATE DATABASE BotBtpDB 
                END; 

                ";
            connection.Execute(query);
        }
    }

    public void CreateTablesIfNotExists()
    {
        using (IDbConnection connection = new SqlConnection(_databaseConnectionString))
        {
            connection.Open();
            string query = @"
                    
                IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Bonds') 
                BEGIN 
                CREATE TABLE Bonds (
                BondId INT IDENTITY (1, 1) PRIMARY KEY NOT NULL,
                Isin NVARCHAR(255) NOT NULL,
                Name NVARCHAR(255) NOT NULL,
                YearlyCoupon FLOAT,
                Expiration DATETIME NOT NULL,
                Url NVARCHAR(255) NOT NULL,
                BondType NVARCHAR(255) NOT NULL,
                IsActive BIT NOT NULL 
                ) 
                END;


                IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'BondsValues') 
                BEGIN 
                CREATE TABLE BondsValues (
                BondValuesId INT IDENTITY (1, 1) PRIMARY KEY NOT NULL,
                BondId INT NOT NULL,
                UtcTime DATETIME NOT NULL,
                Price FLOAT NOT NULL,
                Yield FLOAT NOT NULL,
                FOREIGN KEY (BondId) REFERENCES Bonds(BondId) 
                ) 
				END;
                    
                ";
            connection.Execute(query);
        }
    }

    public async Task<IList<Bond>> GetActiveBondsAsync(string typeOfBond)
    {
        // get all active orders for a bond type
        IList<BondDbModel> bondDbModels = GetBondsBaseData(typeOfBond);

        // get all details only for the active bonds
        IList<BondValuesDbModel> bondValuesDbDtos = GetActiveBondsValues(typeOfBond);


        // merge bonds with their details and map
        List<Bond> bonds = new();
        foreach (var bondDbModel in bondDbModels)
        {
            bondDbModel.HistoricalValues = bondValuesDbDtos.Where(x => x.BondId == bondDbModel.BondId).ToList();
            var bond = _mapper.Map<Bond>(bondDbModel);
            bonds.Add(bond);

        }

        return bonds;
    }
    public void StoreBondSnapshots(IEnumerable<BondSnapshot> bondSnapshots, string typeOfBond)
    {
        using (IDbConnection connection = new SqlConnection(_databaseConnectionString))
        {
            connection.Open();
            using (IDbTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    SetAllBondsToInactive(connection, transaction, typeOfBond);

                    foreach (var bondSnapshot in bondSnapshots)
                    {
                        BondSnapshotDbModel bondSnapshotDbModel = _mapper.Map<BondSnapshotDbModel>(bondSnapshot);

                        int? bondId = GetBondId(connection, bondSnapshotDbModel, transaction);

                        // if bond doesn't exist, create it, else update (maybe values have changed)
                        if (bondId == null)
                        {
                            StoreNewBond(bondSnapshotDbModel, connection, transaction);
                            bondSnapshotDbModel.BondId = GetBondId(connection, bondSnapshotDbModel, transaction);
                        }
                        else
                        {
                            bondSnapshotDbModel.BondId = bondId;
                            UpdateExistingBond(bondSnapshotDbModel, connection, transaction);
                        }
                        StoreBondValues(bondSnapshotDbModel, connection, transaction);
                    }

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }

                transaction.Commit();
            }

        }
    }


    // PRIVATE METHODS
    private IList<BondValuesDbModel> GetActiveBondsValues(string typeOfBond)
    {
        using (IDbConnection conn = new SqlConnection(_databaseConnectionString))
        {
            conn.Open();
            string query = @"
            
                SELECT * FROM Bonds
                INNER JOIN BondsValues ON BondsValues.BondId=Bonds.BondId
                WHERE BondType = @typeOfBond AND IsActive = 1
                
                ";
            var result = conn.Query<BondValuesDbModel>(query, new { typeOfBond }).ToList();
            return result;
        }
    }
    private IList<BondDbModel> GetBondsBaseData(string typeOfBond)
    {
        using (IDbConnection conn = new SqlConnection(_databaseConnectionString))
        {
            conn.Open();
            string query = @"
            
                SELECT * FROM Bonds
                WHERE BondType = @typeOfBond AND IsActive = 1
                ";
            var result = conn.Query<BondDbModel>(query, new { typeOfBond }).ToList();
            return result;
        }
    }

    private void SetAllBondsToInactive(IDbConnection connection, IDbTransaction transaction, string typeOfBond)
    {
        string query = @"
                
            UPDATE Bonds
            SET
            IsActive = 0
            WHERE BondType = @typeOfBond
                
            ";
        connection.Execute(query, new { typeOfBond }, transaction: transaction);
    }

    private void StoreBondValues(BondSnapshotDbModel bondSnapshotDbDto, IDbConnection connection, IDbTransaction transaction)
    {
        string query = @"
            
            INSERT INTO BondsValues (BondId, UtcTime, Price, Yield)
            VALUES (@BondId, @UtcTime, @Price, @Yield)
            
            ";
        connection.Execute(query, bondSnapshotDbDto, transaction);
    }

    private void UpdateExistingBond(BondSnapshotDbModel bondSnapshotDbDto, IDbConnection connection, IDbTransaction transaction)
    {
        string query = @"
                
            UPDATE Bonds
            SET Name = @Name, 
            YearlyCoupon = @YearlyCoupon, 
            Expiration = @Expiration, 
            Url = @Url, 
            BondType = @BondType,
            IsActive = @IsActive
            WHERE BondId = @BondId
                
            ";

        connection.Execute(query, bondSnapshotDbDto, transaction);
    }

    private void StoreNewBond(BondSnapshotDbModel bondSnapshotDbDto, IDbConnection connection, IDbTransaction transaction)
    {
        string query = @"
            
            INSERT INTO Bonds (Isin, Name, YearlyCoupon, Expiration, Url, BondType, IsActive)
            VALUES (@Isin, @Name, @YearlyCoupon, @Expiration, @Url, @BondType, @IsActive)
            
            ";
        connection.Execute(query, bondSnapshotDbDto, transaction);
    }

    private int? GetBondId(IDbConnection connection, BondSnapshotDbModel bondSnapshotDbDto, IDbTransaction transaction)
    {
        string query = "SELECT BondId FROM Bonds WHERE Isin = @Isin";
        int? bondId = connection.ExecuteScalar<int?>(query, bondSnapshotDbDto, transaction);
        return bondId;
    }


}
