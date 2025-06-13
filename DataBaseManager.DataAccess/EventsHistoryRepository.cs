using System.Data;
using Dapper;
using DataBaseManager.DataAccess.Contracts;

namespace DataBaseManager.DataAccess;

public class EventsHistoryRepository : RepositoryBase, IEventsHistoryRepository
{
    public EventsHistoryRepository(IDbTransaction transaction) : base(transaction)
    {
    }
    public void Dispose()
    {
        // W razie potrzeby zaimplementować logikę zwalniania zasobów
        // Na przykład, możma zamknąć transakcję lub połączenie
        Transaction?.Dispose();
    }
    public void FlashAllHistoryData()
    {
        throw new NotImplementedException();
    }

    public void FlashHistoryDataById(int id)
    {
        throw new NotImplementedException();
    }

    public void FlashHistoryDataLess(DateTime date)
    {
        throw new NotImplementedException();
    }

    public void FlashHistoryDataByDateRange(DateTime startDate, DateTime endDate)
    {
        throw new NotImplementedException();
    }

    public void AddEventHistory(string eventDescription)
    {
            if (string.IsNullOrEmpty(eventDescription))
               throw new ArgumentNullException(nameof(eventDescription));

            Connection.ExecuteScalar<int>(
            @"INSERT INTO EventsHistory 
            ([EventData], [DateTime]) VALUES (@EventData, @DateTime);
            SELECT SCOPE_IDENTITY();",
            param: new
            {
                EventData = eventDescription,
                DateTime = DateTime.Now,
            },
            transaction: Transaction
        );
    }
}