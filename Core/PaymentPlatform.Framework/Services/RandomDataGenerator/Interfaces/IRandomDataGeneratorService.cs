using System.Threading.Tasks;

namespace PaymentPlatform.Framework.Services.RandomDataGenerator.Interfaces
{
    /// <summary>
    /// Интерфейс для генератора заполнения базы данных случайными данными.
    /// </summary>
    public interface IRandomDataGeneratorService
    {
        /// <summary>
        /// Добавить новые данные в таблицы аккаунт и профиля.
        /// </summary>
        /// <param name="count">количество.</param>
        Task AddNewAccountsAndProfilesAsync(int count);

        /// <summary>
        /// Добавить новые данные в таблицу продуктов.
        /// </summary>
        /// <param name="count">количество.</param>
        Task AddNewProductsAsync(int count);

        /// <summary>
        /// Добавить новые данные в таблицу транзакций.
        /// </summary>
        /// <param name="count">количество.</param>
        Task AddNewTransactionsAsync(int count);
    }
}