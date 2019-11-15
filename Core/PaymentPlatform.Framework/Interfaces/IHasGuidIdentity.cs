using System;

namespace PaymentPlatform.Framework.Interfaces
{
    /// <summary>
    /// Интерфейс для Id.
    /// </summary>
    public interface IHasGuidIdentity
    {
        /// <summary>
        /// Идентификатор (GUID).
        /// </summary>
        Guid Id { get; set; }
    }
}