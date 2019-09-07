using System.Threading.Tasks;

namespace PaymentPlatform.Initialization.BLL.Interfaces
{
    /// <summary>
    /// Интерфейс для генератора заполнения базы данных случайными данными.
    /// </summary>
    public interface IRandomDataGenerator
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
