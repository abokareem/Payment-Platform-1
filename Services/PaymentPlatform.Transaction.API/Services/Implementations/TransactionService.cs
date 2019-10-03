using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PaymentPlatform.Framework.Enums;
using PaymentPlatform.Framework.Models;
using PaymentPlatform.Framework.Services.RabbitMQ.Interfaces;
using PaymentPlatform.Framework.ViewModels;
using PaymentPlatform.Transaction.API.Models;
using PaymentPlatform.Transaction.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentPlatform.Transaction.API.Services.Implementations
{
	/// <summary>
	/// Сервис транзакций.
	/// </summary>
	public class TransactionService : ITransactionService
	{
		/// <summary>
		/// Контекст транзакций.
		/// </summary>
		private readonly TransactionContext _transactionContext;
		/// <summary>
		/// Экземпляр автомаппера.
		/// </summary>
		private readonly IMapper _mapper;
		/// <summary>
		/// Сервис брокера сообщений.
		/// </summary>
		private readonly IRabbitMQService _rabbitService;

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="transactionContext">Контекст транзакций.</param>
		/// <param name="mapper">Экземпляр автомаппера.</param>
		/// <param name="rabbitService">Сервис брокера сообщений.</param>
		public TransactionService(TransactionContext transactionContext, IMapper mapper, IRabbitMQService rabbitService)
		{
			_transactionContext = transactionContext;
			_mapper = mapper;
			_rabbitService = rabbitService;
			_rabbitService.SetListener("TransactionAPI", OnIncomingMessage);
		}

		/// <summary>
		/// Метод, вызываемый при получении сообщения от брокера.
		/// </summary>
		/// <param name="incomingMessage">Текст сообщения.</param>
		private void OnIncomingMessage(string incomingMessage)
		{
			try
			{
				var incomingObject = JsonConvert.DeserializeObject(incomingMessage) as RabbitMessageModel;

				switch (incomingObject.Sender)
				{
					case "ProductAPI":
						{
							var productReserve = incomingObject.Model as ProductReservedModel;
							var transaction = _transactionContext.Transactions.FirstOrDefault(t => t.Id == productReserve.TransactionId);

							var incomingRabbitMessage = incomingObject.Action;

							switch (incomingRabbitMessage)
							{
								case (int)RabbitMessageActions.Apply:
									{
										transaction.ProductReserveId = productReserve.Id;

										if (transaction.BalanceReserveId != null && transaction.ProductReserveId != null)
										{
											transaction.TransactionSuccess = true;
										}
									}
									break;

								case (int)RabbitMessageActions.Revert:
									{
										transaction.TransactionSuccess = false;
									}
									break;

								default:
									{
										throw new JsonException("Unexpected action.");
									}
							}

							_transactionContext.Entry(transaction).State = EntityState.Modified;
							_transactionContext.SaveChanges();

							break;
						}
					case "ProfileAPI":
						{
							var balanceReserve = incomingObject.Model as BalanceReservedModel;
							var transaction = _transactionContext.Transactions.FirstOrDefault(t => t.Id == balanceReserve.TransactionId);

							var incomingRabbitMessage = incomingObject.Action;

							switch (incomingRabbitMessage)
							{
								case (int)RabbitMessageActions.Apply:
									{
										transaction.BalanceReserveId = balanceReserve.Id;

										if (transaction.BalanceReserveId != null && transaction.ProductReserveId != null)
										{
											transaction.TransactionSuccess = true;
										}
									}
									break;

								case (int)RabbitMessageActions.Revert:
									{
										transaction.TransactionSuccess = false;
									}
									break;

								default:
									{
										throw new JsonException("Unexpected action.");
									}
							}

							_transactionContext.Entry(transaction).State = EntityState.Modified;
							_transactionContext.SaveChanges();

							break;
						}

					default:
						throw new JsonException("Unexpected sender.");
				}
			}
			catch (JsonException)
			{
				//TODO: Вывести в лог
			}
			catch (Exception exc)
			{
				throw new Exception("Unexpected exception", exc);
			}
		}

		/// <inheritdoc/>
		public async Task<(bool success, string message)> AddNewTransactionAsync(TransactionViewModel transaction)
		{
			var newTransaction = _mapper.Map<TransactionModel>(transaction);
			_transactionContext.Entry(newTransaction).State = EntityState.Added;

			await _transactionContext.SaveChangesAsync();
			MakeReserve(newTransaction);

			return (true, "Добавлена новая транзакция.");
		}

		/// <summary>
		/// Создает резерв товара и средств со счета пользователя.
		/// </summary>
		/// <param name="transaction">Транзакция.</param>
		private void MakeReserve(TransactionModel transaction)
		{
			var balanceReserveModel = _mapper.Map<BalanceReservedModel>(transaction);
			var productReserveModel = _mapper.Map<ProductReservedModel>(transaction);

			var messageToProfile = new RabbitMessageModel { Action = (int)RabbitMessageActions.Apply, Sender = "TransactionAPI", Model = balanceReserveModel };
			var messageToProduct = new RabbitMessageModel { Action = (int)RabbitMessageActions.Apply, Sender = "TransactionAPI", Model = productReserveModel };

			_rabbitService.SendMessage(JsonConvert.SerializeObject(messageToProfile), "ProfileAPI");

			_rabbitService.SendMessage(JsonConvert.SerializeObject(messageToProduct), "ProductAPI");
		}

		/// <summary>
		/// Отменяет резеврирование товара и средств пользователя.
		/// </summary>
		/// <param name="transaction">Транзакция.</param>
		private void RevertReserve(TransactionModel transaction)
		{
			var balanceReserveModel = _mapper.Map<BalanceReservedModel>(transaction);
			var productReserveModel = _mapper.Map<ProductReservedModel>(transaction);

			var messageToProfile = new RabbitMessageModel { Action = (int)RabbitMessageActions.Revert, Sender = "TransactionAPI", Model = balanceReserveModel };
			var messageToProduct = new RabbitMessageModel { Action = (int)RabbitMessageActions.Revert, Sender = "TransactionAPI", Model = productReserveModel };

			_rabbitService.SendMessage(JsonConvert.SerializeObject(messageToProfile), "ProfileAPI");

			_rabbitService.SendMessage(JsonConvert.SerializeObject(messageToProduct), "ProductAPI");
		}

		/// <inheritdoc/>
		public async Task<TransactionViewModel> GetTransactionByIdAsync(Guid id)
		{
			var transaction = await _transactionContext.Transactions.FirstOrDefaultAsync(t => t.Id == id);

			return _mapper.Map<TransactionViewModel>(transaction);
		}

		/// <inheritdoc/>
		public async Task<ICollection<TransactionViewModel>> GetTransactionsAsync(int? take = null, int? skip = null)
		{
			var transactions = _transactionContext.Transactions.Select(t => t);

			if (take > 0)
			{
				transactions = transactions.Take((int)take);
			}
			if (skip > 0)
			{
				transactions = transactions.Skip((int)skip);
			}

			var transactionResult = await transactions.ToListAsync();
			var transactionResultViewModels = _mapper.Map<List<TransactionViewModel>>(transactionResult);

			return transactionResultViewModels;
		}

		/// <inheritdoc/>
		public async Task<(bool success, string message)> RevertTransactionByIdAsync(Guid id)
		{
			var transaction = await _transactionContext.Transactions.FirstOrDefaultAsync(t => t.Id == id);

			RevertReserve(transaction);
			transaction.TransactionSuccess = false;

			return (true, "Transaction canceled successfully.");
		}

		/// <inheritdoc/>
		public async Task<TransactionViewModel> UpdateTransactionAsync(TransactionViewModel transaction)
		{
			var transactionInDatabase = await _transactionContext.Transactions.FirstOrDefaultAsync(t => t.Id == transaction.Id);

			transactionInDatabase.ProductId = transaction.ProductId;
			transactionInDatabase.ProfileId = transaction.ProfileId;
			transactionInDatabase.TransactionTime = transaction.TransactionTime;
			transactionInDatabase.Status = transaction.Status;
			transactionInDatabase.TotalCost = transaction.TotalCost;

			_transactionContext.Transactions.Update(transactionInDatabase);
			await _transactionContext.SaveChangesAsync();

			return _mapper.Map<TransactionViewModel>(transactionInDatabase);
		}

	}
}
