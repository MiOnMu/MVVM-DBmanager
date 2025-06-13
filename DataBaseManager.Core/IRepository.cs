namespace DataBaseManager.Core;

/// <summary>
/// To jest generyczny interfejs, przeznaczony do pracy z modelami dowolnego typu
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IRepository<T> : IDisposable where T : class
{
    IEnumerable<T> GetItemsList(); // pobieranie wszystkich obiektów
    T GetItemById(int id);            // pobieranie jednego obiektu po id
    void Create(T item);          // tworzenie obiektu
    void Update(T item);          // aktualizacja obiektu
    void Delete(int id);          // usuwanie obiektu po id
    void Save();                  // zapisywanie zmian
}