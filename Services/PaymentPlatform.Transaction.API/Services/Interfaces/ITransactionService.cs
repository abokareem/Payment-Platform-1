using PaymentPlatform.Framework.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentPlatform.Transaction.API.Services.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса транзакций.
    /// </summary>
    public interface ITransactionService
    {
        /// <summary>
        /// Получить список транзакций.
        /// </summary>
        /// <param name="take">Параметр пагинации - взять.</param>
        /// <param name="skip">Параметр пагинации - пропустить.</param>
        /// <returns>Коллекцию TransactionViewModel.</returns>
        Task<IEnumerable<TransactionViewModel>> GetTransactionsAsync(int? take = null, int? skip = null);

        /// <summary>
        /// Получить транзакцию по Id.
        /// </summary>
        /// <param name="id">Id транзакции.</param>
        /// <returns>Модель TransactionViewModel.</returns>
        Task<TransactionViewModel> GetTransactionByIdAsync(Guid id);

        /// <summary>
        /// Добавить новую транзакцию.
        /// </summary>
        /// <param name="transaction">Модель представления транзакции.</param>
        /// <returns>(успешность выполнения, сообщение)</returns>
        Task<(bool success, string message)> AddNewTransactionAsync(TransactionViewModel transaction);

        /// <summary>
        /// Отменить транзакцию по Id.
        /// </summary>
        /// <param name="id">Id транзакции.</param>
        /// <returns>(успешность выполнения, сообщение)</returns>
        Task<(bool success, string message)> RevertTransactionByIdAsync(Guid id);

        /// <summary>
        /// Обновить транзакцию.
        /// </summary>
        /// <param name="transaction">Модель представления транзакции.</param>
        /// <returns>Обновленную модель представленя.</returns>
        Task<bool> UpdateTransactionAsync(TransactionViewModel transaction);
    }
}